using Blazored.TextEditor;
using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Brand;
using LiteCommerce.Admin.Models.Business.Category;
using LiteCommerce.Admin.Models.Business.Product;
using LiteCommerce.Admin.Models.Business.ProductAttribute;
using LiteCommerce.Admin.Models.Business.ProductTemplate;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Net.Http.Headers;

namespace LiteCommerce.Admin.Pages.Catalog.Products
{
    public partial class UpsertProduct
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
        private ISnackbar Snackbar { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

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

        private string? _selectedTemplateId;
        private string? _selectedAvailableAttributeId;
        private string? _thumbnailPreviewUrl;
        private List<string> _productImagePreviewUrls = new();

        private BlazoredTextEditor _shortDescriptionEditor = null!;
        private BlazoredTextEditor _descriptionEditor = null!;
        private bool _editorsLoaded = false;

        protected override async Task OnInitializedAsync()
        {
            _breadcrumbs = new()
            {
                new("Home", href: "/", icon: Icons.Material.Filled.Home),
                new("Catalog", href: null, disabled: true),
                new("Products", href: "/products"),
                new(_isEditMode ? "Edit Product" : "Add Product", href: null, disabled: true)
            };

            await Task.WhenAll(
                LoadBrands(),
                LoadBasicCategories(),
                LoadProductTemplates(),
                LoadAllAttributes(),
                LoadAllProducts()
            );

            if (_isEditMode)
            {
                await LoadProduct();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (_isEditMode && !_editorsLoaded && !_loading && _productForm.ShortDescription != null)
            {
                _editorsLoaded = true;
                if (!string.IsNullOrEmpty(_productForm.ShortDescription))
                    await _shortDescriptionEditor.LoadContent(_productForm.ShortDescription);
                if (!string.IsNullOrEmpty(_productForm.Description))
                    await _descriptionEditor.LoadContent(_productForm.Description);
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

        private async Task LoadAllProducts()
        {
            var response = await ProductApi.GetProductsAsync(1, 1000);
            if (response.IsSuccess)
            {
                _allProducts = response.Data;
            }
        }

        private async Task LoadProduct()
        {
            _loading = true;
            var response = await ProductApi.GetProductAsync(Id!);

            if (!response.IsSuccess)
            {
                Snackbar.Add(response.Message ?? SystemMessages.ErrorOccurred, Severity.Error);
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

            if (file.Size > FileUploadConstants.MaxImageSize)
            {
                Snackbar.Add(FileUploadConstants.ImageSizeExceededMessage, Severity.Error);
                return;
            }

            if (!FileUploadConstants.AllowedImageTypes.Contains(file.ContentType))
            {
                Snackbar.Add(FileUploadConstants.InvalidImageTypeMessage, Severity.Error);
                return;
            }

            try
            {
                var buffer = new byte[file.Size];
                await file.OpenReadStream(FileUploadConstants.MaxImageSize).ReadAsync(buffer);
                var imageBase64 = Convert.ToBase64String(buffer);
                _thumbnailPreviewUrl = $"data:{file.ContentType};base64,{imageBase64}";
                _createProductForm.ThumbnailImage = file;
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error loading image: {ex.Message}", Severity.Error);
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
                if (file.Size > FileUploadConstants.MaxImageSize)
                {
                    Snackbar.Add($"{file.Name}: {FileUploadConstants.ImageSizeExceededMessage}", Severity.Error);
                    continue;
                }

                if (!FileUploadConstants.AllowedImageTypes.Contains(file.ContentType))
                {
                    Snackbar.Add($"{file.Name}: {FileUploadConstants.InvalidImageTypeMessage}", Severity.Error);
                    continue;
                }

                try
                {
                    var buffer = new byte[file.Size];
                    await file.OpenReadStream(FileUploadConstants.MaxImageSize).ReadAsync(buffer);
                    var imageBase64 = Convert.ToBase64String(buffer);

                    _createProductForm.ProductImages.Add(file);
                    _productImagePreviewUrls.Add($"data:{file.ContentType};base64,{imageBase64}");
                }
                catch (Exception ex)
                {
                    Snackbar.Add($"Error loading {file.Name}: {ex.Message}", Severity.Error);
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
                if (file.Size > FileUploadConstants.MaxDocumentSize)
                {
                    Snackbar.Add($"{file.Name}: {FileUploadConstants.DocumentSizeExceededMessage}", Severity.Error);
                    continue;
                }

                if (!FileUploadConstants.AllowedDocumentTypes.Contains(file.ContentType))
                {
                    Snackbar.Add($"{file.Name}: {FileUploadConstants.InvalidDocumentTypeMessage}", Severity.Error);
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

            if (!string.IsNullOrEmpty(_productForm.Id))
                content.Add(new StringContent(_productForm.Id), "Product.Id");

            content.Add(new StringContent(_productForm.Name ?? string.Empty), "Product.Name");
            content.Add(new StringContent(_productForm.ShortDescription ?? string.Empty), "Product.ShortDescription");
            content.Add(new StringContent(_productForm.Description ?? string.Empty), "Product.Description");
            content.Add(new StringContent(_productForm.MetaTitle ?? string.Empty), "Product.MetaTitle");
            content.Add(new StringContent(_productForm.MetaKeywords ?? string.Empty), "Product.MetaKeywords");
            content.Add(new StringContent(_productForm.MetaDescription ?? string.Empty), "Product.MetaDescription");
            content.Add(new StringContent(_productForm.Sku ?? string.Empty), "Product.Sku");
            content.Add(new StringContent(_productForm.Gtin ?? string.Empty), "Product.Gtin");
            content.Add(new StringContent((_productForm.Price ?? 0).ToString()), "Product.Price");
            content.Add(new StringContent(_productForm.IsPublished.ToString()), "Product.IsPublished");
            content.Add(new StringContent(_productForm.IsFeatured.ToString()), "Product.IsFeatured");
            content.Add(new StringContent(_productForm.IsCallForPricing.ToString()), "Product.IsCallForPricing");
            content.Add(new StringContent(_productForm.IsAllowToOrder.ToString()), "Product.IsAllowToOrder");

            if (_productForm.OldPrice.HasValue)
                content.Add(new StringContent(_productForm.OldPrice.Value.ToString()), "Product.OldPrice");

            if (_productForm.SpecialPrice.HasValue)
                content.Add(new StringContent(_productForm.SpecialPrice.Value.ToString()), "Product.SpecialPrice");

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
                var fileContent = new StreamContent(_createProductForm.ThumbnailImage.OpenReadStream(FileUploadConstants.MaxImageSize));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(_createProductForm.ThumbnailImage.ContentType);
                content.Add(fileContent, "ThumbnailImage", _createProductForm.ThumbnailImage.Name);
            }

            foreach (var imageFile in _createProductForm.ProductImages)
            {
                var fileContent = new StreamContent(imageFile.OpenReadStream(FileUploadConstants.MaxImageSize));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
                content.Add(fileContent, "ProductImages", imageFile.Name);
            }

            foreach (var docFile in _createProductForm.ProductDocuments)
            {
                var fileContent = new StreamContent(docFile.OpenReadStream(FileUploadConstants.MaxDocumentSize));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(docFile.ContentType);
                content.Add(fileContent, "ProductDocuments", docFile.Name);
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
            var content = await CreateMultipartFormDataContent();

            var response = _isEditMode
                ? await ProductApi.UpdateProductAsync(content)
                : await ProductApi.CreateProductAsync(content);

            if (response.IsSuccess)
            {
                var successMessage = _isEditMode
                    ? SystemMessages.UpdateDataSuccess
                    : SystemMessages.AddDataSuccess;

                Snackbar.Add(successMessage, Severity.Success);
                NavigationManager.NavigateTo("/products");
            }
            else
            {
                Snackbar.Add(response.Message ?? SystemMessages.ErrorOccurred, Severity.Error);
            }

            _loading = false;
        }

        private void Cancel()
        {
            NavigationManager.NavigateTo("/products");
        }

        #endregion
    }
}
