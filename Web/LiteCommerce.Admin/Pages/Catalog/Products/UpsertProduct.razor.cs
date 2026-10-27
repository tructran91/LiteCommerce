using Blazored.TextEditor;
using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Brand;
using LiteCommerce.Admin.Models.Business.Category;
using LiteCommerce.Admin.Models.Business.Product;
using LiteCommerce.Admin.Models.Business.ProductAttribute;
using LiteCommerce.Admin.Models.Business.ProductOption;
using LiteCommerce.Admin.Models.Business.ProductTemplate;
using LiteCommerce.Admin.Models.Common;
using LiteCommerce.Admin.Shared.Components;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;
using System.Globalization;
using System.Net.Http.Headers;

namespace LiteCommerce.Admin.Pages.Catalog.Products
{
    public partial class UpsertProduct : IDisposable
    {
        [Parameter]
        public string? Id { get; set; }

        [Inject]
        private IProductApi ProductApi { get; set; }

        [Inject]
        private ICategoryApi CategoryApi { get; set; }

        [Inject]
        private IBrandApi BrandApi { get; set; }

        [Inject]
        private IProductTemplateApi ProductTemplateApi { get; set; }

        [Inject]
        private IProductAttributeApi ProductAttributeApi { get; set; }

        [Inject]
        private IProductOptionApi ProductOptionApi { get; set; }

        [Inject]
        private IDialogService DialogService { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Inject]
        private FileUploadSettings FileUploadSettings { get; set; }

        private List<BreadcrumbItem> _breadcrumbs = new();
        private bool _isEditMode => !string.IsNullOrEmpty(Id);
        private bool _loading = false;
        private MudForm _form = null!;

        private CreateProductFormModel _createProductForm = new();
        private ProductFormModel _productForm => _createProductForm.Product;

        private List<BrandResponse> _brands = new();
        private List<BasicCategoryResponse> _basicCategories = new();
        private List<ProductTemplateResponse> _productTemplates = new();
        private List<ProductAttributeResponse> _allAttributes = new();
        private List<ProductAttributeResponse> _availableAttributes = new();
        private List<BasicProductResponse> _allProducts = new();
        private List<ProductOptionResponse> _allOptions = new();

        // Copies of Catalog.Core.Constants.VariantLimits, which is what actually
        // enforces them. Change both.
        private const int MaxOptions = 3;
        private const int MaxValuesPerOption = 50;
        private const int MaxVariants = 200;

        private bool HasVariants => _productForm.Variants.Any();

        private List<ProductOptionResponse> AvailableOptions =>
            _allOptions.Where(o => _productForm.Options.All(x => x.Id != o.Id?.ToString())).ToList();

        private string? _selectedTemplateId;
        private string? _selectedAvailableAttributeId;
        private string? _selectedAvailableOptionId;
        private string? _thumbnailPreviewUrl;
        private List<string> _productImagePreviewUrls = new();

        private BlazoredTextEditor _shortDescriptionEditor = null!;
        private BlazoredTextEditor _descriptionEditor = null!;
        private bool _editorsLoaded = false;
        private DotNetObjectReference<UpsertProduct>? _dotNetRef;
        private bool _imageHandlerRegistered = false;
        private string? _contentTempId;

        protected override async Task OnInitializedAsync()
        {
            _breadcrumbs = new()
            {
                new(L["Common_Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(L["Menu_Catalog"], href: null, disabled: true),
                new(L["Product_Title"], href: "/products"),
                new(_isEditMode ? L["Product_EditTitle"] : L["Product_AddTitle"], href: null, disabled: true)
            };

            await Task.WhenAll(
                LoadBrands(),
                LoadBasicCategories(),
                LoadProductTemplates(),
                LoadAllAttributes(),
                LoadAllOptions(),
                LoadAllProducts()
            );

            if (_isEditMode)
            {
                await LoadProduct();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _dotNetRef = DotNetObjectReference.Create(this);
                // Delay to ensure Quill is fully initialized
                await Task.Delay(500);
                await RegisterDescriptionImageHandler();
            }

            if (_isEditMode && !_editorsLoaded && !_loading && _productForm.ShortDescription != null)
            {
                _editorsLoaded = true;
                if (!string.IsNullOrEmpty(_productForm.ShortDescription))
                    await _shortDescriptionEditor.LoadContent(_productForm.ShortDescription);
                if (!string.IsNullOrEmpty(_productForm.Description))
                    await _descriptionEditor.LoadContent(_productForm.Description);
            }
        }

        private async Task RegisterDescriptionImageHandler()
        {
            if (_imageHandlerRegistered) return;

            try
            {
                await JSRuntime.InvokeVoidAsync("QuillImageUpload.registerImageHandler", ".long-desc-container", _dotNetRef);
                _imageHandlerRegistered = true;
            }
            catch
            {
                // Quill may not be ready yet, will retry on next render if needed
            }
        }

        [JSInvokable]
        public async Task<string?> UploadContentImage(string fileName, string contentType, string base64Data)
        {
            try
            {
                var bytes = Convert.FromBase64String(base64Data);
                var content = new MultipartFormDataContent();

                var fileContent = new ByteArrayContent(bytes);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                content.Add(fileContent, "file", fileName);

                string productId;
                bool isNewProduct;

                if (_isEditMode)
                {
                    productId = _productForm.Id;
                    isNewProduct = false;
                }
                else
                {
                    if (string.IsNullOrEmpty(_contentTempId))
                    {
                        _contentTempId = Guid.NewGuid().ToString();
                    }
                    productId = _contentTempId;
                    isNewProduct = true;
                }

                content.Add(new StringContent(productId), "productId");
                content.Add(new StringContent(isNewProduct.ToString()), "isNewProduct");

                var response = await ProductApi.UploadContentImageAsync(content);
                if (!response.IsSuccess)
                {
                    Snackbar.Add(L["Product_UploadImageFailed", response.GetErrorMessage(L["Common_ErrorOccurred"])], Severity.Error);
                    return null;
                }

                return response.Data;
            }
            catch (Exception ex)
            {
                Snackbar.Add(L["Product_UploadImageFailed", ex.Message], Severity.Error);
                return null;
            }
        }

        #region Data Loading

        private async Task LoadBrands()
        {
            var response = await BrandApi.GetBrandsAsync(1, 100);
            if (response.IsSuccess)
            {
                _brands = response.Data;
            }
        }

        private async Task LoadBasicCategories()
        {
            var response = await CategoryApi.GetBasicCategoriesAsync();
            if (response.IsSuccess)
            {
                _basicCategories = response.Data;
            }
        }

        private async Task LoadProductTemplates()
        {
            var response = await ProductTemplateApi.GetProductTemplatesAsync(1, 100);
            if (response.IsSuccess)
            {
                _productTemplates = response.Data;
            }
        }

        private async Task LoadAllAttributes()
        {
            var response = await ProductAttributeApi.GetProductAttributesAsync(1, 100);
            if (response.IsSuccess)
            {
                _allAttributes = response.Data;
                RefreshAvailableAttributes();
            }
        }

        private async Task LoadAllOptions()
        {
            var response = await ProductOptionApi.GetProductOptionsAsync(1, 100);
            if (response.IsSuccess)
            {
                _allOptions = response.Data ?? new();
            }
        }

        private async Task LoadAllProducts()
        {
            const int pageSize = 100;
            var currentPage = 1;
            var products = new List<BasicProductResponse>();

            while (true)
            {
                var response = await ProductApi.GetProductsAsync(currentPage, pageSize);
                if (!response.IsSuccess)
                    return;

                products.AddRange(response.Data ?? []);

                if (currentPage >= (response.Pagination?.TotalPages ?? 1))
                    break;

                currentPage++;
            }

            _allProducts = products;
        }

        private async Task LoadProduct()
        {
            _loading = true;

            BaseResponse<ProductFormModel> response;
            try
            {
                response = await ProductApi.GetProductAsync(Id!);
            }
            catch (Exception ex)
            {
                Snackbar.Add(L["Common_ErrorWithDetail", ex.Message], Severity.Error);
                _loading = false;
                return;
            }

            if (!response.IsSuccess)
            {
                Snackbar.Add(response.GetErrorMessage(L["Common_ErrorOccurred"]), Severity.Error);
                _loading = false;
                return;
            }

            var data = response.Data;

            _productForm.Id = data.Id;
            _productForm.Name = data.Name;
            _productForm.BrandId = data.BrandId;
            _productForm.ShortDescription = data.ShortDescription;
            _productForm.Description = data.Description;
            _productForm.MetaTitle = data.MetaTitle;
            _productForm.MetaKeywords = data.MetaKeywords;
            _productForm.MetaDescription = data.MetaDescription;
            _productForm.IsPublished = data.IsPublished;
            _productForm.Price = data.Price;
            _productForm.OldPrice = data.OldPrice;
            _productForm.SpecialPrice = data.SpecialPrice;
            _productForm.SpecialPriceStart = data.SpecialPriceStart;
            _productForm.SpecialPriceEnd = data.SpecialPriceEnd;
            _productForm.IsFeatured = data.IsFeatured;
            _productForm.IsCallForPricing = data.IsCallForPricing;
            _productForm.IsAllowToOrder = data.IsAllowToOrder;
            _productForm.Sku = data.Sku;
            _productForm.Gtin = data.Gtin;
            _productForm.ThumbnailImageUrl = data.ThumbnailImageUrl;
            _productForm.HasVariants = data.HasVariants;
            _productForm.PriceMax = data.PriceMax;
            _productForm.StockQuantity = data.StockQuantity;
            _productForm.Options = data.Options ?? new List<ProductOptionFormItem>();
            _productForm.Variants = data.Variants ?? new List<ProductVariantFormItem>();

            if (!string.IsNullOrEmpty(_productForm.ThumbnailImageUrl))
            {
                _thumbnailPreviewUrl = _productForm.ThumbnailImageUrl;
            }

            _productForm.CategoryIds.Clear();
            if (data.CategoryIds != null)
            {
                foreach (var catId in data.CategoryIds)
                {
                    _productForm.CategoryIds.Add(catId);
                }
            }

            if (data.Attributes != null)
            {
                _productForm.Attributes = data.Attributes.Select(a => new ProductAttributeFormItem
                {
                    Id = a.Id,
                    Name = a.Name,
                    Value = a.Value,
                    GroupName = a.GroupName
                }).ToList();
                RefreshAvailableAttributes();
            }

            if (data.ProductImages != null)
            {
                _productForm.ProductImages = data.ProductImages.Select(m => new ProductMediaFormItem
                {
                    Id = m.Id,
                    Caption = m.Caption,
                    MediaUrl = m.MediaUrl
                }).ToList();
            }

            if (data.ProductDocuments != null)
            {
                _productForm.ProductDocuments = data.ProductDocuments.Select(m => new ProductMediaFormItem
                {
                    Id = m.Id,
                    Caption = m.Caption,
                    MediaUrl = m.MediaUrl
                }).ToList();
            }

            if (data.RelatedProducts != null)
            {
                _productForm.RelatedProducts = data.RelatedProducts.Select(p => new ProductLinkFormItem
                {
                    Id = p.Id,
                    Name = p.Name,
                    IsPublished = p.IsPublished
                }).ToList();
            }

            if (data.CrossSellProducts != null)
            {
                _productForm.CrossSellProducts = data.CrossSellProducts.Select(p => new ProductLinkFormItem
                {
                    Id = p.Id,
                    Name = p.Name,
                    IsPublished = p.IsPublished
                }).ToList();
            }

            _loading = false;
        }

        #endregion

        #region Product Attributes

        private void RefreshAvailableAttributes()
        {
            var usedIds = _productForm.Attributes.Select(a => a.Id).ToHashSet();
            _availableAttributes = _allAttributes.Where(a => !usedIds.Contains(a.Id)).ToList();
        }

        private void ApplyTemplate()
        {
            if (string.IsNullOrEmpty(_selectedTemplateId))
                return;

            var template = _productTemplates.FirstOrDefault(t => t.Id == _selectedTemplateId);
            if (template == null)
                return;

            _productForm.Attributes.Clear();

            foreach (var attr in template.ProductAttributes)
            {
                var fullAttr = _allAttributes.FirstOrDefault(a => a.Id == attr.Id);
                _productForm.Attributes.Add(new ProductAttributeFormItem
                {
                    Id = attr.Id,
                    Name = attr.Name,
                    Value = string.Empty,
                    GroupName = fullAttr?.GroupName ?? string.Empty
                });
            }

            RefreshAvailableAttributes();
        }

        private void AddAvailableAttribute()
        {
            if (string.IsNullOrEmpty(_selectedAvailableAttributeId))
                return;

            var attr = _allAttributes.FirstOrDefault(a => a.Id == _selectedAvailableAttributeId);
            if (attr == null || _productForm.Attributes.Any(a => a.Id == attr.Id))
                return;

            _productForm.Attributes.Add(new ProductAttributeFormItem
            {
                Id = attr.Id,
                Name = attr.Name,
                Value = string.Empty,
                GroupName = attr.GroupName ?? string.Empty
            });

            _selectedAvailableAttributeId = null;
            RefreshAvailableAttributes();
        }

        private void RemoveAttribute(string attributeId)
        {
            var item = _productForm.Attributes.FirstOrDefault(a => a.Id == attributeId);
            if (item != null)
            {
                _productForm.Attributes.Remove(item);
                RefreshAvailableAttributes();
            }
        }

        #endregion

        #region Product Options and Variants

        private void AddOption()
        {
            if (string.IsNullOrEmpty(_selectedAvailableOptionId))
                return;

            var option = _allOptions.FirstOrDefault(o => o.Id?.ToString() == _selectedAvailableOptionId);
            if (option == null || _productForm.Options.Any(o => o.Id == _selectedAvailableOptionId))
                return;

            _productForm.Options.Add(new ProductOptionFormItem
            {
                Id = _selectedAvailableOptionId,
                Name = option.Name,
                DisplayType = "text",
                // Start with one blank row so the axis is never saved with no values.
                Values = new List<ProductOptionValueFormItem> { new() }
            });

            _selectedAvailableOptionId = null;
        }

        // Dropping an axis does not invalidate the grid, it collapses it: "Red / S" and
        // "Red / M" both become "Red". The first row of each collapsed group keeps its
        // id, price and stock; only the now-duplicate rows go.
        private async Task RemoveOption(ProductOptionFormItem option)
        {
            var remaining = _productForm.Options.Where(o => o != option).ToList();
            var remainingIds = remaining.Select(o => o.Id).ToHashSet(StringComparer.OrdinalIgnoreCase);

            var lost = remainingIds.Count == 0
                ? _productForm.Variants.Count
                : _productForm.Variants.Count - _productForm.Variants
                    .Select(v => BuildCombinationKey(v, remainingIds))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Count();

            if (lost > 0 && !await ConfirmVariantLossAsync(lost))
                return;

            _productForm.Options.Remove(option);
            _productForm.Variants = ProjectVariants(_productForm.Variants, remaining);
        }

        private static void AddOptionValue(ProductOptionFormItem option)
        {
            if (option.Values.Count >= MaxValuesPerOption)
                return;

            option.Values.Add(new ProductOptionValueFormItem());
        }

        // Only the variants that actually use this value go; the rest are untouched.
        private async Task RemoveOptionValue(ProductOptionFormItem option, ProductOptionValueFormItem value)
        {
            var key = value.Key?.Trim();
            var affected = string.IsNullOrWhiteSpace(key)
                ? 0
                : _productForm.Variants.Count(v => v.OptionCombinations.Any(c =>
                    string.Equals(c.OptionId, option.Id, StringComparison.OrdinalIgnoreCase)
                    && string.Equals(c.ValueKey?.Trim(), key, StringComparison.OrdinalIgnoreCase)));

            if (affected > 0 && !await ConfirmVariantLossAsync(affected))
                return;

            option.Values.Remove(value);

            if (affected > 0)
            {
                _productForm.Variants = _productForm.Variants
                    .Where(v => !v.OptionCombinations.Any(c =>
                        string.Equals(c.OptionId, option.Id, StringComparison.OrdinalIgnoreCase)
                        && string.Equals(c.ValueKey?.Trim(), key, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
                Reindex(_productForm.Variants);
            }
        }

        // Rewrites every variant against the axes that remain, dropping the rows whose
        // combination is no longer distinct. Keeps the first of each group so its id,
        // price and stock survive.
        private static List<ProductVariantFormItem> ProjectVariants(
            IList<ProductVariantFormItem> variants,
            IList<ProductOptionFormItem> remainingOptions)
        {
            if (remainingOptions.Count == 0)
            {
                return new List<ProductVariantFormItem>();
            }

            var order = remainingOptions
                .Select((o, i) => (o.Id, Index: i))
                .ToDictionary(x => x.Id, x => x.Index, StringComparer.OrdinalIgnoreCase);

            var projected = new List<ProductVariantFormItem>();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var variant in variants)
            {
                var kept = variant.OptionCombinations
                    .Where(c => order.ContainsKey(c.OptionId))
                    .OrderBy(c => order[c.OptionId])
                    .ToList();

                // A variant that no longer covers every remaining axis cannot be rebuilt.
                if (kept.Count != remainingOptions.Count)
                {
                    continue;
                }

                foreach (var combination in kept)
                {
                    combination.SortIndex = order[combination.OptionId];
                }

                variant.OptionCombinations = kept;

                if (!seen.Add(BuildCombinationKey(variant)))
                {
                    continue;
                }

                // The server rebuilds both from the surviving combination.
                variant.Name = null;
                variant.NormalizedName = null;
                projected.Add(variant);
            }

            Reindex(projected);
            return projected;
        }

        private static void Reindex(IList<ProductVariantFormItem> variants)
        {
            for (var i = 0; i < variants.Count; i++)
            {
                variants[i].DisplayOrder = i;
            }
        }

        private async Task<bool> ConfirmVariantLossAsync(int affectedCount)
        {
            var parameters = new DialogParameters
            {
                { nameof(ConfirmActionDialog.Message), string.Format(L["Product_VariantLossWarning"], affectedCount) },
                { nameof(ConfirmActionDialog.Color), Color.Warning }
            };

            var dialog = await DialogService.ShowAsync<ConfirmActionDialog>(
                L["Product_VariantLossTitle"].Value, parameters);
            var result = await dialog.Result;

            return result is { Canceled: false };
        }

        // Rebuilds the grid as the cartesian product of the declared axes. Prices and
        // stock already typed in are carried over by combination, so regenerating after
        // adding one value does not wipe the existing rows.
        private async Task GenerateVariants()
        {
            var axes = _productForm.Options
                .Select(o => new
                {
                    Option = o,
                    Values = o.Values.Where(v => !string.IsNullOrWhiteSpace(v.Key)).ToList()
                })
                .Where(a => a.Values.Count > 0)
                .ToList();

            if (axes.Count == 0)
            {
                Snackbar.Add(L["Product_OptionsNeedValues"], Severity.Warning);
                return;
            }

            var total = axes.Aggregate(1, (acc, a) => acc * a.Values.Count);
            if (total > MaxVariants)
            {
                Snackbar.Add(string.Format(L["Product_TooManyVariants"], total, MaxVariants), Severity.Error);
                return;
            }

            // Rows whose combination no longer exists are the only ones lost, so only
            // ask when there actually are some.
            var reachable = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            CollectCombinationKeys(axes.Select(a => (a.Option.Id, a.Values)).ToList(), 0, [], reachable);

            var dropped = _productForm.Variants.Count(v => !reachable.Contains(BuildCombinationKey(v)));
            if (dropped > 0 && !await ConfirmVariantLossAsync(dropped))
                return;

            var previous = _productForm.Variants.ToDictionary(
                v => BuildCombinationKey(v),
                v => v,
                StringComparer.OrdinalIgnoreCase);

            var generated = new List<ProductVariantFormItem>();
            var combination = new ProductVariantOptionFormItem[axes.Count];

            void Build(int axisIndex)
            {
                var axis = axes[axisIndex];

                foreach (var value in axis.Values)
                {
                    combination[axisIndex] = new ProductVariantOptionFormItem
                    {
                        OptionId = axis.Option.Id,
                        OptionName = axis.Option.Name,
                        ValueKey = value.Key.Trim(),
                        ValueDisplay = string.IsNullOrWhiteSpace(value.Display) ? value.Key.Trim() : value.Display.Trim(),
                        SortIndex = axisIndex
                    };

                    if (axisIndex == axes.Count - 1)
                    {
                        generated.Add(BuildVariant(combination.ToArray(), previous, generated.Count));
                    }
                    else
                    {
                        Build(axisIndex + 1);
                    }
                }
            }

            Build(0);

            _productForm.Variants = generated;
        }

        // Every key the cartesian product would produce, in the same format
        // BuildCombinationKey uses, so the two can be compared directly.
        private static void CollectCombinationKeys(
            IReadOnlyList<(string OptionId, List<ProductOptionValueFormItem> Values)> axes,
            int axisIndex,
            List<string> parts,
            HashSet<string> results)
        {
            if (axisIndex == axes.Count)
            {
                results.Add(string.Join("|", parts.OrderBy(p => p, StringComparer.OrdinalIgnoreCase)));
                return;
            }

            var (optionId, values) = axes[axisIndex];
            foreach (var value in values)
            {
                parts.Add($"{optionId}:{value.Key?.Trim().ToLowerInvariant()}");
                CollectCombinationKeys(axes, axisIndex + 1, parts, results);
                parts.RemoveAt(parts.Count - 1);
            }
        }

        private ProductVariantFormItem BuildVariant(
            ProductVariantOptionFormItem[] combination,
            IReadOnlyDictionary<string, ProductVariantFormItem> previous,
            int displayOrder)
        {
            var variant = new ProductVariantFormItem
            {
                OptionCombinations = combination.ToList(),
                DisplayOrder = displayOrder
            };

            if (previous.TryGetValue(BuildCombinationKey(variant), out var existing))
            {
                // Keep the id so the server updates the row instead of replacing it.
                variant.Id = existing.Id;
                variant.Name = existing.Name;
                variant.NormalizedName = existing.NormalizedName;
                variant.Sku = existing.Sku;
                variant.Gtin = existing.Gtin;
                variant.Price = existing.Price;
                variant.OldPrice = existing.OldPrice;
                variant.SpecialPrice = existing.SpecialPrice;
                variant.SpecialPriceStart = existing.SpecialPriceStart;
                variant.SpecialPriceEnd = existing.SpecialPriceEnd;
                variant.StockQuantity = existing.StockQuantity;
                variant.IsActive = existing.IsActive;
                variant.ThumbnailImageUrl = existing.ThumbnailImageUrl;
            }
            else
            {
                // A new combination starts from the product level price.
                variant.Price = _productForm.Price ?? 0;
                variant.OldPrice = _productForm.OldPrice;
                variant.IsActive = true;
            }

            return variant;
        }

        // Ordered by option id so the key does not depend on axis ordering. Passing
        // onlyOptionIds yields the key the variant would have once the other axes go,
        // which is how the collapse count is worked out before anything is removed.
        private static string BuildCombinationKey(
            ProductVariantFormItem variant,
            IReadOnlySet<string>? onlyOptionIds = null)
            => string.Join("|", variant.OptionCombinations
                .Where(c => onlyOptionIds == null || onlyOptionIds.Contains(c.OptionId))
                .OrderBy(c => c.OptionId, StringComparer.OrdinalIgnoreCase)
                .Select(c => $"{c.OptionId}:{c.ValueKey?.Trim().ToLowerInvariant()}"));

        private void RemoveVariant(ProductVariantFormItem variant)
        {
            _productForm.Variants.Remove(variant);
        }

        #endregion

        #region Category Mapping

        private void OnCategoryCheckedChanged(string categoryId, bool isChecked)
        {
            if (isChecked)
            {
                if (!_productForm.CategoryIds.Contains(categoryId))
                    _productForm.CategoryIds.Add(categoryId);
            }
            else
            {
                _productForm.CategoryIds.Remove(categoryId);
            }
        }

        #endregion

        #region Related & Cross-sell Products

        private Task<IEnumerable<BasicProductResponse>> SearchRelatedProducts(string value, CancellationToken ct)
        {
            var excludeIds = _productForm.RelatedProducts.Select(p => p.Id).ToHashSet();
            if (!string.IsNullOrEmpty(_productForm.Id))
                excludeIds.Add(_productForm.Id);

            var results = _allProducts
                .Where(p => !excludeIds.Contains(p.Id.ToString()))
                .Where(p => string.IsNullOrEmpty(value) || p.Name.Contains(value, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult(results);
        }

        private void AddRelatedProduct(BasicProductResponse? product)
        {
            if (product == null || _productForm.RelatedProducts.Any(p => p.Id == product.Id.ToString()))
                return;

            _productForm.RelatedProducts.Add(new ProductLinkFormItem
            {
                Id = product.Id.ToString()!,
                Name = product.Name,
                IsPublished = product.IsPublished
            });
        }

        private void RemoveRelatedProduct(string productId)
        {
            var item = _productForm.RelatedProducts.FirstOrDefault(p => p.Id == productId);
            if (item != null)
                _productForm.RelatedProducts.Remove(item);
        }

        private Task<IEnumerable<BasicProductResponse>> SearchCrossSellProducts(string value, CancellationToken ct)
        {
            var excludeIds = _productForm.CrossSellProducts.Select(p => p.Id).ToHashSet();
            if (!string.IsNullOrEmpty(_productForm.Id))
                excludeIds.Add(_productForm.Id);

            var results = _allProducts
                .Where(p => !excludeIds.Contains(p.Id.ToString()))
                .Where(p => string.IsNullOrEmpty(value) || p.Name.Contains(value, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult(results);
        }

        private void AddCrossSellProduct(BasicProductResponse? product)
        {
            if (product == null || _productForm.CrossSellProducts.Any(p => p.Id == product.Id.ToString()))
                return;

            _productForm.CrossSellProducts.Add(new ProductLinkFormItem
            {
                Id = product.Id.ToString()!,
                Name = product.Name,
                IsPublished = product.IsPublished
            });
        }

        private void RemoveCrossSellProduct(string productId)
        {
            var item = _productForm.CrossSellProducts.FirstOrDefault(p => p.Id == productId);
            if (item != null)
                _productForm.CrossSellProducts.Remove(item);
        }

        #endregion

        #region File Uploads

        private async Task OnThumbnailSelected(InputFileChangeEventArgs e)
        {
            var file = e.File;

            if (file.Size > FileUploadSettings.MaxImageSize)
            {
                Snackbar.Add(L["Upload_ImageSizeExceeded"], Severity.Error);
                return;
            }

            if (!FileUploadConstants.AllowedImageTypes.Contains(file.ContentType))
            {
                Snackbar.Add(L["Upload_InvalidImageType"], Severity.Error);
                return;
            }

            try
            {
                var buffer = new byte[file.Size];
                await file.OpenReadStream(FileUploadSettings.MaxImageSize).ReadAsync(buffer);
                var imageBase64 = Convert.ToBase64String(buffer);
                _thumbnailPreviewUrl = $"data:{file.ContentType};base64,{imageBase64}";
                _createProductForm.ThumbnailImage = file;
            }
            catch (Exception ex)
            {
                Snackbar.Add(L["Product_ImageLoadFailed", ex.Message], Severity.Error);
                _createProductForm.ThumbnailImage = null;
                _thumbnailPreviewUrl = null;
            }
        }

        private void RemoveThumbnail()
        {
            _createProductForm.ThumbnailImage = null;
            _thumbnailPreviewUrl = null;
            _productForm.ThumbnailImageUrl = null;
        }

        private async Task OnProductImagesSelected(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles())
            {
                if (file.Size > FileUploadSettings.MaxImageSize)
                {
                    Snackbar.Add(L["Product_ImageTooLargeNamed", file.Name, L["Upload_ImageSizeExceeded"]], Severity.Error);
                    continue;
                }

                if (!FileUploadConstants.AllowedImageTypes.Contains(file.ContentType))
                {
                    Snackbar.Add(L["Product_ImageTooLargeNamed", file.Name, L["Upload_InvalidImageType"]], Severity.Error);
                    continue;
                }

                try
                {
                    var buffer = new byte[file.Size];
                    await file.OpenReadStream(FileUploadSettings.MaxImageSize).ReadAsync(buffer);
                    var imageBase64 = Convert.ToBase64String(buffer);

                    _createProductForm.ProductImages.Add(file);
                    _productImagePreviewUrls.Add($"data:{file.ContentType};base64,{imageBase64}");
                }
                catch (Exception ex)
                {
                    Snackbar.Add(L["Product_ImageLoadFailedNamed", file.Name, ex.Message], Severity.Error);
                }
            }
        }

        private void RemoveProductImage(int index)
        {
            _createProductForm.ProductImages.RemoveAt(index);
            _productImagePreviewUrls.RemoveAt(index);
        }

        private void RemoveExistingProductImage(string mediaId)
        {
            var item = _productForm.ProductImages.FirstOrDefault(x => x.Id == mediaId);
            if (item != null)
            {
                _productForm.ProductImages.Remove(item);
                _productForm.DeletedMediaIds.Add(mediaId);
            }
        }

        private void OnProductDocumentsSelected(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles())
            {
                if (file.Size > FileUploadSettings.MaxDocumentSize)
                {
                    Snackbar.Add(L["Product_ImageTooLargeNamed", file.Name, L["Upload_DocumentSizeExceeded"]], Severity.Error);
                    continue;
                }

                if (!FileUploadConstants.AllowedDocumentTypes.Contains(file.ContentType))
                {
                    Snackbar.Add(L["Product_ImageTooLargeNamed", file.Name, L["Upload_InvalidDocumentType"]], Severity.Error);
                    continue;
                }

                _createProductForm.ProductDocuments.Add(file);
            }
        }

        private void RemoveProductDocument(int index)
        {
            _createProductForm.ProductDocuments.RemoveAt(index);
        }

        private void RemoveExistingProductDocument(string mediaId)
        {
            var item = _productForm.ProductDocuments.FirstOrDefault(x => x.Id == mediaId);
            if (item != null)
            {
                _productForm.ProductDocuments.Remove(item);
                _productForm.DeletedMediaIds.Add(mediaId);
            }
        }

        #endregion

        #region Form Submission

        private async Task<MultipartFormDataContent> CreateMultipartFormDataContent()
        {
            var content = new MultipartFormDataContent();

            content.Add(new StringContent(_productForm.Name ?? string.Empty), "Product.Name");
            content.Add(new StringContent(_productForm.ShortDescription ?? string.Empty), "Product.ShortDescription");
            content.Add(new StringContent(_productForm.Description ?? string.Empty), "Product.Description");
            content.Add(new StringContent(_productForm.MetaTitle ?? string.Empty), "Product.MetaTitle");
            content.Add(new StringContent(_productForm.MetaKeywords ?? string.Empty), "Product.MetaKeywords");
            content.Add(new StringContent(_productForm.MetaDescription ?? string.Empty), "Product.MetaDescription");
            content.Add(new StringContent(_productForm.Sku ?? string.Empty), "Product.Sku");
            content.Add(new StringContent(_productForm.Gtin ?? string.Empty), "Product.Gtin");
            // The UI culture may be vi-VN, where the decimal separator is a comma, but the
            // API model binder parses invariant. Without this, "1234,56" binds as 123456.
            content.Add(new StringContent((_productForm.Price ?? 0).ToString(CultureInfo.InvariantCulture)), "Product.Price");
            // Omitting this binds 0 onto the entity, and RecalculateAggregates only
            // restores the value for products that have variants.
            content.Add(new StringContent(_productForm.StockQuantity.ToString(CultureInfo.InvariantCulture)), "Product.StockQuantity");
            content.Add(new StringContent(_productForm.IsPublished.ToString()), "Product.IsPublished");
            content.Add(new StringContent(_productForm.IsFeatured.ToString()), "Product.IsFeatured");
            content.Add(new StringContent(_productForm.IsCallForPricing.ToString()), "Product.IsCallForPricing");
            content.Add(new StringContent(_productForm.IsAllowToOrder.ToString()), "Product.IsAllowToOrder");

            if (_productForm.OldPrice.HasValue)
                content.Add(new StringContent(_productForm.OldPrice.Value.ToString(CultureInfo.InvariantCulture)), "Product.OldPrice");

            if (_productForm.SpecialPrice.HasValue)
                content.Add(new StringContent(_productForm.SpecialPrice.Value.ToString(CultureInfo.InvariantCulture)), "Product.SpecialPrice");

            if (_productForm.SpecialPriceStart.HasValue)
                content.Add(new StringContent(_productForm.SpecialPriceStart.Value.ToString("o")), "Product.SpecialPriceStart");

            if (_productForm.SpecialPriceEnd.HasValue)
                content.Add(new StringContent(_productForm.SpecialPriceEnd.Value.ToString("o")), "Product.SpecialPriceEnd");

            if (!string.IsNullOrEmpty(_productForm.BrandId))
                content.Add(new StringContent(_productForm.BrandId), "Product.BrandId");

            for (var i = 0; i < _productForm.CategoryIds.Count; i++)
                content.Add(new StringContent(_productForm.CategoryIds[i]), $"Product.CategoryIds[{i}]");

            for (var i = 0; i < _productForm.DeletedMediaIds.Count; i++)
                content.Add(new StringContent(_productForm.DeletedMediaIds[i]), $"Product.DeletedMediaIds[{i}]");

            for (var i = 0; i < _productForm.Attributes.Count; i++)
            {
                content.Add(new StringContent(_productForm.Attributes[i].Id), $"Product.Attributes[{i}].Id");
                content.Add(new StringContent(_productForm.Attributes[i].Name ?? string.Empty), $"Product.Attributes[{i}].Name");
                content.Add(new StringContent(_productForm.Attributes[i].Value ?? string.Empty), $"Product.Attributes[{i}].Value");
                content.Add(new StringContent(_productForm.Attributes[i].GroupName ?? string.Empty), $"Product.Attributes[{i}].GroupName");
            }

            for (var i = 0; i < _productForm.Options.Count; i++)
            {
                var option = _productForm.Options[i];
                content.Add(new StringContent(option.Id), $"Product.Options[{i}].Id");
                content.Add(new StringContent(option.Name ?? string.Empty), $"Product.Options[{i}].Name");
                content.Add(new StringContent(option.DisplayType ?? "text"), $"Product.Options[{i}].DisplayType");

                for (var j = 0; j < option.Values.Count; j++)
                {
                    var value = option.Values[j];
                    content.Add(new StringContent(value.Key ?? string.Empty), $"Product.Options[{i}].Values[{j}].Key");
                    content.Add(new StringContent(value.Display ?? string.Empty), $"Product.Options[{i}].Values[{j}].Display");
                }
            }

            for (var i = 0; i < _productForm.Variants.Count; i++)
            {
                var variant = _productForm.Variants[i];

                if (!string.IsNullOrEmpty(variant.Id))
                    content.Add(new StringContent(variant.Id), $"Product.Variants[{i}].Id");

                content.Add(new StringContent(variant.Name ?? string.Empty), $"Product.Variants[{i}].Name");
                content.Add(new StringContent(variant.Sku ?? string.Empty), $"Product.Variants[{i}].Sku");
                content.Add(new StringContent(variant.Gtin ?? string.Empty), $"Product.Variants[{i}].Gtin");
                content.Add(new StringContent(variant.Price.ToString(CultureInfo.InvariantCulture)), $"Product.Variants[{i}].Price");
                content.Add(new StringContent(variant.StockQuantity.ToString(CultureInfo.InvariantCulture)), $"Product.Variants[{i}].StockQuantity");
                content.Add(new StringContent(variant.IsActive.ToString()), $"Product.Variants[{i}].IsActive");
                content.Add(new StringContent(variant.DisplayOrder.ToString(CultureInfo.InvariantCulture)), $"Product.Variants[{i}].DisplayOrder");

                if (variant.OldPrice.HasValue)
                    content.Add(new StringContent(variant.OldPrice.Value.ToString(CultureInfo.InvariantCulture)), $"Product.Variants[{i}].OldPrice");

                if (variant.SpecialPrice.HasValue)
                    content.Add(new StringContent(variant.SpecialPrice.Value.ToString(CultureInfo.InvariantCulture)), $"Product.Variants[{i}].SpecialPrice");

                if (variant.SpecialPriceStart.HasValue)
                    content.Add(new StringContent(variant.SpecialPriceStart.Value.ToString("o")), $"Product.Variants[{i}].SpecialPriceStart");

                if (variant.SpecialPriceEnd.HasValue)
                    content.Add(new StringContent(variant.SpecialPriceEnd.Value.ToString("o")), $"Product.Variants[{i}].SpecialPriceEnd");

                for (var j = 0; j < variant.OptionCombinations.Count; j++)
                {
                    var combination = variant.OptionCombinations[j];
                    content.Add(new StringContent(combination.OptionId), $"Product.Variants[{i}].OptionCombinations[{j}].OptionId");
                    content.Add(new StringContent(combination.ValueKey ?? string.Empty), $"Product.Variants[{i}].OptionCombinations[{j}].ValueKey");
                    content.Add(new StringContent(combination.SortIndex.ToString(CultureInfo.InvariantCulture)), $"Product.Variants[{i}].OptionCombinations[{j}].SortIndex");
                }
            }

            for (var i = 0; i < _productForm.RelatedProducts.Count; i++)
            {
                content.Add(new StringContent(_productForm.RelatedProducts[i].Id), $"Product.RelatedProducts[{i}].Id");
                content.Add(new StringContent(_productForm.RelatedProducts[i].Name ?? string.Empty), $"Product.RelatedProducts[{i}].Name");
                content.Add(new StringContent(_productForm.RelatedProducts[i].IsPublished.ToString()), $"Product.RelatedProducts[{i}].IsPublished");
            }

            for (var i = 0; i < _productForm.CrossSellProducts.Count; i++)
            {
                content.Add(new StringContent(_productForm.CrossSellProducts[i].Id), $"Product.CrossSellProducts[{i}].Id");
                content.Add(new StringContent(_productForm.CrossSellProducts[i].Name ?? string.Empty), $"Product.CrossSellProducts[{i}].Name");
                content.Add(new StringContent(_productForm.CrossSellProducts[i].IsPublished.ToString()), $"Product.CrossSellProducts[{i}].IsPublished");
            }

            if (_createProductForm.ThumbnailImage != null)
            {
                var fileContent = new StreamContent(_createProductForm.ThumbnailImage.OpenReadStream(FileUploadSettings.MaxImageSize));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(_createProductForm.ThumbnailImage.ContentType);
                content.Add(fileContent, "ThumbnailImage", _createProductForm.ThumbnailImage.Name);
            }

            foreach (var imageFile in _createProductForm.ProductImages)
            {
                var fileContent = new StreamContent(imageFile.OpenReadStream(FileUploadSettings.MaxImageSize));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
                content.Add(fileContent, "ProductImages", imageFile.Name);
            }

            foreach (var docFile in _createProductForm.ProductDocuments)
            {
                var fileContent = new StreamContent(docFile.OpenReadStream(FileUploadSettings.MaxDocumentSize));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(docFile.ContentType);
                content.Add(fileContent, "ProductDocuments", docFile.Name);
            }

            if (!_isEditMode && !string.IsNullOrEmpty(_contentTempId))
            {
                content.Add(new StringContent(_contentTempId), "ContentTempId");
            }

            return content;
        }

        private async Task FormSubmitted()
        {
            await _form.Validate();
            if (!_form.IsValid) return;

            _productForm.ShortDescription = await _shortDescriptionEditor.GetHTML();
            _productForm.Description = await _descriptionEditor.GetHTML();

            _loading = true;

            try
            {
                var content = await CreateMultipartFormDataContent();

                var response = _isEditMode
                    ? await ProductApi.UpdateProductAsync(Id!, content)
                    : await ProductApi.CreateProductAsync(content);

                if (response.IsSuccess)
                {
                    var successMessage = _isEditMode
                        ? L["Common_UpdateDataSuccess"]
                        : L["Common_AddDataSuccess"];

                    Snackbar.Add(successMessage, Severity.Success);
                    NavigationManager.NavigateTo("/products");
                }
                else
                {
                    Snackbar.Add(response.GetErrorMessage(L["Common_ErrorOccurred"]), Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add(L["Common_ErrorWithDetail", ex.Message], Severity.Error);
            }
            finally
            {
                _loading = false;
            }
        }

        private void Cancel()
        {
            NavigationManager.NavigateTo("/products");
        }

        #endregion

        public void Dispose()
        {
            _dotNetRef?.Dispose();
        }
    }
}
