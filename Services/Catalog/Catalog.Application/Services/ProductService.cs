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
                Name = x.Option.Name,
                DisplayType = x.DisplayType,
                Values = JsonSerializer.Deserialize<IList<ProductOptionValueViewModel>>(x.Value)
            }).ToList();
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
                    productTarget.AddOptionValue(new ProductOptionValue
                    {
                        OptionId = optionId,
                        DisplayType = optionVm.DisplayType,
                        Value = JsonSerializer.Serialize(optionVm.Values),
                        SortIndex = optionIndex,
                        CreatedDate = DateTime.UtcNow
                    });
                }
                else
                {
                    existing.DisplayType = optionVm.DisplayType;
                    existing.Value = JsonSerializer.Serialize(optionVm.Values);
                    existing.SortIndex = optionIndex;
                }
                optionIndex++;
            }

            var newOptionIds = newOptions.Select(x => Guid.Parse(x.Id)).ToHashSet();
            var optionsToRemove = productTarget.OptionValues.Where(x => !newOptionIds.Contains(x.OptionId)).ToList();
            foreach (var item in optionsToRemove)
            {
                productTarget.OptionValues.Remove(item);
            }
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
    }
}
