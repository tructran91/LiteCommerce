using AutoMapper;
using Catalog.Application.Extensions;
using Catalog.Application.Responses;
using Catalog.Application.ViewModels;
using Catalog.Core.Entities;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Constants;
using System.Text.Json;

namespace Catalog.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IMediaService _mediaService;
        private readonly IMapper _mapper;

        public ProductService(IMediaService mediaService, IMapper mapper)
        {
            _mediaService = mediaService;
            _mapper = mapper;
        }

        public ProductPriceHistory CreatePriceHistory(Product product)
        {
            return new ProductPriceHistory
            {
                Product = product,
                Price = product.Price,
                OldPrice = product.OldPrice,
                SpecialPrice = product.SpecialPrice,
                SpecialPriceStart = product.SpecialPriceStart,
                SpecialPriceEnd = product.SpecialPriceEnd,
                CreatedDate = DateTime.UtcNow
            };
        }

        public ProductResponse BuildProductResponse(Product product)
        {
            var response = _mapper.Map<ProductResponse>(product);
            var subFolder = product.Id.ToStoragePath(StorageFolder.Product);

            response.CategoryIds = product.Categories.Select(x => x.CategoryId.ToString()).ToList();
            response.ThumbnailImageUrl = _mediaService.GetThumbnailUrl(product.ThumbnailImage, subFolder);
            response.Options = product.OptionValues.OrderBy(x => x.SortIndex).Select(x => new ProductOptionViewModel
            {
                Id = x.OptionId.ToString(),
                Name = x.Option?.Name,
                DisplayType = x.DisplayType,
                Values = x.Items
                    .OrderBy(i => i.SortIndex)
                    .Select(i => new ProductOptionValueViewModel { Key = i.Key, Display = i.Display })
                    .ToList()
            }).ToList();
            response.Variants = BuildVariants(product);
            response.Attributes = product.AttributeValues.Select(x => new ProductAttributeViewModel
            {
                AttributeValueId = x.Id.ToString(),
                Id = x.AttributeId.ToString(),
                Name = x.Attribute.Name,
                GroupName = x.Attribute.Group.Name,
                Value = x.Value
            }).ToList();

            response.ProductImages = product.Medias
                .Where(x => x.Media.MediaType == MediaType.Image)
                .Select(x => new ProductMediaViewModel
                {
                    Id = x.Id.ToString(),
                    Caption = x.Media.Caption,
                    MediaUrl = _mediaService.GetThumbnailUrl(x.Media, subFolder)
                })
                .ToList();
            response.ProductDocuments = product.Medias
                .Where(x => x.Media.MediaType == MediaType.File)
                .Select(x => new ProductMediaViewModel
                {
                    Id = x.Id.ToString(),
                    Caption = x.Media.Caption,
                    MediaUrl = _mediaService.GetMediaUrl(x.Media, subFolder)
                })
                .ToList();
            response.RelatedProducts = BuildProductLinks(product, ProductLinkType.Related);
            response.CrossSellProducts = BuildProductLinks(product, ProductLinkType.CrossSell);

            return response;
        }

        private List<ProductVariantViewModel> BuildVariants(Product product)
        {
            var subFolder = product.Id.ToStoragePath(StorageFolder.Product);

            var axisOrder = product.OptionValues
                .ToDictionary(x => x.OptionId, x => x.SortIndex);

            return product.Variants
                .OrderBy(v => v.DisplayOrder)
                .ThenBy(v => v.NormalizedName)
                .Select(v => new ProductVariantViewModel
                {
                    Id = v.Id.ToString(),
                    Name = v.Name,
                    NormalizedName = v.NormalizedName,
                    Sku = v.Sku,
                    Gtin = v.Gtin,
                    Price = v.Price,
                    OldPrice = v.OldPrice,
                    SpecialPrice = v.SpecialPrice,
                    SpecialPriceStart = v.SpecialPriceStart,
                    SpecialPriceEnd = v.SpecialPriceEnd,
                    StockQuantity = v.StockQuantity,
                    IsActive = v.IsActive,
                    DisplayOrder = v.DisplayOrder,
                    ThumbnailImageUrl = _mediaService.GetThumbnailUrl(v.ThumbnailImage, subFolder),
                    OptionCombinations = v.OptionValues
                        .OrderBy(ov => axisOrder.TryGetValue(ov.OptionId, out var idx) ? idx : ov.SortIndex)
                        .Select(ov => new ProductVariantOptionViewModel
                        {
                            OptionId = ov.OptionId.ToString(),
                            OptionName = ov.Option?.Name,
                            ValueKey = ov.ProductOptionValueItem?.Key,
                            ValueDisplay = ov.ProductOptionValueItem?.Display,
                            SortIndex = ov.SortIndex
                        })
                        .ToList()
                })
                .ToList();
        }

        private static List<ProductLinkViewModel> BuildProductLinks(Product product, ProductLinkType linkType)
        {
            return product.ProductLinks
                .Where(x => x.LinkType == linkType && x.LinkedProduct != null && !x.LinkedProduct.IsDeleted)
                .OrderBy(x => x.LinkedProductId)
                .Select(x => new ProductLinkViewModel
                {
                    Id = x.LinkedProductId.ToString(),
                    Name = x.LinkedProduct.Name,
                    IsPublished = x.LinkedProduct.IsPublished
                })
                .ToList();
        }

        public void AddOrDeleteProductLinks(ProductViewModel productSource, Product productTarget)
        {
            // Related Products - Add new
            foreach (var relatedProductVm in productSource.RelatedProducts ?? [])
            {
                var productLink = productTarget.ProductLinks
                    .Where(x => x.LinkType == ProductLinkType.Related)
                    .FirstOrDefault(x => x.LinkedProductId == Guid.Parse(relatedProductVm.Id));

                if (productLink == null)
                {
                    productTarget.AddProductLinks(new ProductLink
                    {
                        LinkType = ProductLinkType.Related,
                        Product = productTarget,
                        LinkedProductId = Guid.Parse(relatedProductVm.Id),
                        CreatedDate = DateTime.UtcNow
                    });
                }
            }

            // Related Products - Remove deleted
            var relatedToRemove = productTarget.ProductLinks
                .Where(x => x.LinkType == ProductLinkType.Related)
                .Where(x => (productSource.RelatedProducts ?? []).All(vm => Guid.Parse(vm.Id) != x.LinkedProductId))
                .ToList();
            foreach (var link in relatedToRemove)
            {
                productTarget.ProductLinks.Remove(link);
            }

            // Cross-sell Products - Add new
            foreach (var crossSellProductVm in productSource.CrossSellProducts ?? [])
            {
                var productLink = productTarget.ProductLinks
                    .Where(x => x.LinkType == ProductLinkType.CrossSell)
                    .FirstOrDefault(x => x.LinkedProductId == Guid.Parse(crossSellProductVm.Id));

                if (productLink == null)
                {
                    productTarget.AddProductLinks(new ProductLink
                    {
                        LinkType = ProductLinkType.CrossSell,
                        Product = productTarget,
                        LinkedProductId = Guid.Parse(crossSellProductVm.Id),
                        CreatedDate = DateTime.UtcNow
                    });
                }
            }

            // Cross-sell Products - Remove deleted
            var crossSellToRemove = productTarget.ProductLinks
                .Where(x => x.LinkType == ProductLinkType.CrossSell)
                .Where(x => (productSource.CrossSellProducts ?? []).All(vm => Guid.Parse(vm.Id) != x.LinkedProductId))
                .ToList();
            foreach (var link in crossSellToRemove)
            {
                productTarget.ProductLinks.Remove(link);
            }
        }

        public void AddOrDeleteOptions(ProductViewModel productSource, Product productTarget)
        {
            var newOptions = productSource.Options ?? [];
            var optionIndex = 0;

            foreach (var optionVm in newOptions)
            {
                var optionId = Guid.Parse(optionVm.Id);
                var existing = productTarget.OptionValues.FirstOrDefault(x => x.OptionId == optionId);
                if (existing == null)
                {
                    existing = new ProductOptionValue
                    {
                        OptionId = optionId,
                        DisplayType = optionVm.DisplayType,
                        SortIndex = optionIndex,
                        CreatedDate = DateTime.UtcNow
                    };
                    productTarget.AddOptionValue(existing);
                }
                else
                {
                    existing.DisplayType = optionVm.DisplayType;
                    existing.SortIndex = optionIndex;
                }

                SyncOptionValueItems(optionVm.Values ?? [], existing);
                optionIndex++;
            }

            var newOptionIds = newOptions.Select(x => Guid.Parse(x.Id)).ToHashSet();
            var optionsToRemove = productTarget.OptionValues.Where(x => !newOptionIds.Contains(x.OptionId)).ToList();
            foreach (var item in optionsToRemove)
            {
                productTarget.OptionValues.Remove(item);
            }
        }

        // Matching on Key, not on position, so renaming Display keeps the row and every
        // variant pointing at it. Dropping a key drops the row, and AddOrDeleteVariants
        // then drops the variants that referenced it.
        private static void SyncOptionValueItems(IList<ProductOptionValueViewModel> sourceValues, ProductOptionValue target)
        {
            var itemIndex = 0;

            foreach (var valueVm in sourceValues)
            {
                var key = NormalizeKey(valueVm.Key);
                var existing = target.Items.FirstOrDefault(x => string.Equals(x.Key, key, StringComparison.OrdinalIgnoreCase));
                if (existing == null)
                {
                    target.AddItem(new ProductOptionValueItem
                    {
                        Key = key,
                        Display = string.IsNullOrWhiteSpace(valueVm.Display) ? valueVm.Key : valueVm.Display,
                        SortIndex = itemIndex,
                        CreatedDate = DateTime.UtcNow
                    });
                }
                else
                {
                    existing.Display = string.IsNullOrWhiteSpace(valueVm.Display) ? valueVm.Key : valueVm.Display;
                    existing.SortIndex = itemIndex;
                }
                itemIndex++;
            }

            var newKeys = sourceValues.Select(x => NormalizeKey(x.Key)).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var itemsToRemove = target.Items.Where(x => !newKeys.Contains(x.Key)).ToList();
            foreach (var item in itemsToRemove)
            {
                target.Items.Remove(item);
            }
        }

        // Slugified so a key stays the same across accents and casing.
        // ProductVariantRules has a copy; the two must agree.
        private static string NormalizeKey(string key)
        {
            var slug = (key ?? string.Empty).Slugify();
            return string.IsNullOrWhiteSpace(slug) ? (key ?? string.Empty).Trim().ToLowerInvariant() : slug;
        }

        public void AddOrDeleteAttributes(ProductViewModel productSource, Product productTarget)
        {
            var newAttributes = productSource.Attributes ?? [];

            foreach (var attrVm in newAttributes)
            {
                var attrId = Guid.Parse(attrVm.Id);
                var existing = productTarget.AttributeValues.FirstOrDefault(x => x.AttributeId == attrId);
                if (existing == null)
                {
                    productTarget.AddAttributeValue(new ProductAttributeValue
                    {
                        AttributeId = attrId,
                        Value = attrVm.Value ?? ""
                    });
                }
                else
                {
                    existing.Value = attrVm.Value ?? "";
                }
            }

            var newAttrIds = newAttributes.Select(x => Guid.Parse(x.Id)).ToHashSet();
            var attrsToRemove = productTarget.AttributeValues.Where(x => !newAttrIds.Contains(x.AttributeId)).ToList();
            foreach (var item in attrsToRemove)
            {
                productTarget.AttributeValues.Remove(item);
            }
        }

        public void AddOrDeleteCategories(ProductViewModel productSource, Product productTarget)
        {
            var newCategoryIds = (productSource.CategoryIds ?? []).Select(Guid.Parse).ToHashSet();

            foreach (var categoryId in newCategoryIds)
            {
                if (!productTarget.Categories.Any(x => x.CategoryId == categoryId))
                {
                    productTarget.AddCategory(new ProductCategory
                    {
                        CategoryId = categoryId,
                        CreatedDate = DateTime.UtcNow
                    });
                }
            }

            var categoriesToRemove = productTarget.Categories.Where(x => !newCategoryIds.Contains(x.CategoryId)).ToList();
            foreach (var item in categoriesToRemove)
            {
                productTarget.Categories.Remove(item);
            }
        }

        public void AddOrDeleteVariants(ProductViewModel productSource, Product productTarget)
        {
            var sourceVariants = productSource.Variants ?? [];

            // The axes come from what AddOrDeleteOptions just synced, not from the
            // request, so a variant can only reference a value the product declares.
            var axes = productTarget.OptionValues.OrderBy(x => x.SortIndex).ToList();
            var itemLookup = axes.ToDictionary(
                axis => axis.OptionId,
                axis => axis.Items.ToDictionary(i => i.Key, i => i, StringComparer.OrdinalIgnoreCase));

            var seenNormalizedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var displayOrder = 0;

            foreach (var variantVm in sourceVariants)
            {
                var resolved = ResolveCombination(variantVm, axes, itemLookup);
                if (resolved == null)
                {
                    // Unknown axis or value. The validator already returns 400 for a real
                    // mistake, so this only drops a stale payload rather than persisting it.
                    continue;
                }

                var normalizedName = BuildNormalizedName(resolved);
                if (!seenNormalizedNames.Add(normalizedName))
                {
                    continue;
                }

                var existing = ResolveExistingVariant(productTarget, variantVm.Id, normalizedName);

                if (existing == null)
                {
                    existing = new ProductVariant
                    {
                        NormalizedName = normalizedName,
                        CreatedDate = DateTime.UtcNow
                    };
                    productTarget.AddVariant(existing);
                }

                existing.NormalizedName = normalizedName;
                existing.Name = string.IsNullOrWhiteSpace(variantVm.Name)
                    ? BuildVariantDisplayName(productTarget.Name, resolved)
                    : variantVm.Name.Trim();
                existing.Sku = string.IsNullOrWhiteSpace(variantVm.Sku) ? null : variantVm.Sku.Trim();
                existing.Gtin = string.IsNullOrWhiteSpace(variantVm.Gtin) ? null : variantVm.Gtin.Trim();
                existing.Price = variantVm.Price;
                existing.OldPrice = variantVm.OldPrice;
                existing.SpecialPrice = variantVm.SpecialPrice;
                existing.SpecialPriceStart = variantVm.SpecialPriceStart;
                existing.SpecialPriceEnd = variantVm.SpecialPriceEnd;
                existing.StockQuantity = variantVm.StockQuantity;
                existing.IsActive = variantVm.IsActive;
                existing.DisplayOrder = displayOrder++;

                SyncVariantOptionValues(resolved, existing);
            }

            var variantsToRemove = productTarget.Variants
                .Where(v => !seenNormalizedNames.Contains(v.NormalizedName))
                .ToList();
            foreach (var variant in variantsToRemove)
            {
                productTarget.Variants.Remove(variant);
            }
        }

        // Resolves the submitted combination to the product's own option value items, in
        // axis order. Null when an axis is missing, unknown, duplicated or extra.
        private static List<(ProductOptionValue Axis, ProductOptionValueItem Item)>? ResolveCombination(
            ProductVariantViewModel variantVm,
            List<ProductOptionValue> axes,
            Dictionary<Guid, Dictionary<string, ProductOptionValueItem>> itemLookup)
        {
            if (axes.Count == 0)
            {
                return null;
            }

            var combinations = variantVm.OptionCombinations ?? [];
            var byOptionId = new Dictionary<Guid, string>();

            foreach (var combination in combinations)
            {
                if (!Guid.TryParse(combination.OptionId, out var optionId))
                {
                    return null;
                }
                // TryAdd also rejects the same axis supplied twice.
                if (!byOptionId.TryAdd(optionId, NormalizeKey(combination.ValueKey)))
                {
                    return null;
                }
            }

            var resolved = new List<(ProductOptionValue, ProductOptionValueItem)>(axes.Count);
            foreach (var axis in axes)
            {
                if (!byOptionId.TryGetValue(axis.OptionId, out var key))
                {
                    return null;
                }
                if (!itemLookup[axis.OptionId].TryGetValue(key, out var item))
                {
                    return null;
                }
                resolved.Add((axis, item));
            }

            // Count check rejects extra axes the product does not declare.
            return byOptionId.Count == axes.Count ? resolved : null;
        }

        private static string BuildNormalizedName(List<(ProductOptionValue Axis, ProductOptionValueItem Item)> resolved)
            => string.Join("-", resolved.Select(x => x.Item.Key));

        private static string BuildVariantDisplayName(
            string productName,
            List<(ProductOptionValue Axis, ProductOptionValueItem Item)> resolved)
            => $"{productName} {string.Join(" / ", resolved.Select(x => x.Item.Display))}".Trim();

        // Id first, then the combination. Never Name: renaming a variant must not orphan it.
        private static ProductVariant? ResolveExistingVariant(Product product, string? rawId, string normalizedName)
        {
            if (!string.IsNullOrWhiteSpace(rawId) && Guid.TryParse(rawId, out var variantId) && variantId != Guid.Empty)
            {
                var byId = product.Variants.FirstOrDefault(v => v.Id == variantId);
                if (byId != null)
                {
                    return byId;
                }
            }

            return product.Variants.FirstOrDefault(
                v => string.Equals(v.NormalizedName, normalizedName, StringComparison.OrdinalIgnoreCase));
        }

        private static void SyncVariantOptionValues(
            List<(ProductOptionValue Axis, ProductOptionValueItem Item)> resolved,
            ProductVariant variant)
        {
            foreach (var (axis, item) in resolved)
            {
                var existing = variant.OptionValues.FirstOrDefault(x => x.OptionId == axis.OptionId);
                if (existing == null)
                {
                    variant.AddOptionValue(new ProductVariantOptionValue
                    {
                        OptionId = axis.OptionId,
                        // The navigation, not the id: a newly added item has no id yet,
                        // and EF fills the FK when it saves.
                        ProductOptionValueItem = item,
                        SortIndex = axis.SortIndex,
                        CreatedDate = DateTime.UtcNow
                    });
                }
                else
                {
                    existing.ProductOptionValueItem = item;
                    existing.SortIndex = axis.SortIndex;
                }
            }

            var keptOptionIds = resolved.Select(x => x.Axis.OptionId).ToHashSet();
            var toRemove = variant.OptionValues.Where(x => !keptOptionIds.Contains(x.OptionId)).ToList();
            foreach (var optionValue in toRemove)
            {
                variant.OptionValues.Remove(optionValue);
            }
        }

        public void RecalculateAggregates(Product product)
        {
            var activeVariants = product.Variants.Where(v => v.IsActive && !v.IsDeleted).ToList();

            product.HasVariants = product.Variants.Count > 0;

            if (!product.HasVariants)
            {
                // The admin's own figures stand. PriceMax still mirrors Price so a list
                // screen can render a range for every product without a special case.
                product.PriceMax = product.Price;
                return;
            }

            if (activeVariants.Count == 0)
            {
                // Nothing sellable. Keep the last known price rather than flipping to 0.
                product.PriceMax = product.Price;
                product.StockQuantity = 0;
                return;
            }

            var cheapest = activeVariants.OrderBy(v => v.Price).First();

            product.Price = cheapest.Price;
            product.PriceMax = activeVariants.Max(v => v.Price);
            // From the same variant as Price, so the struck-through figure on a listing
            // always belongs to the price shown next to it.
            product.OldPrice = cheapest.OldPrice;
            product.StockQuantity = activeVariants.Sum(v => v.StockQuantity);
        }
    }
}
