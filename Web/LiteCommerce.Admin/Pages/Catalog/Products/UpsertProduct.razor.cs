using Blazored.Toast.Services;
using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Helpers;
using LiteCommerce.Admin.Models.Application;
using LiteCommerce.Admin.Models.Business.Brand;
using LiteCommerce.Admin.Models.Business.Category;
using LiteCommerce.Admin.Models.Business.Product;
using LiteCommerce.Admin.Models.Business.ProductAttributeGroup;
using LiteCommerce.Admin.Models.Business.ProductTemplate;
using LiteCommerce.Admin.Models.Common;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using System.Net.Http.Headers;

namespace LiteCommerce.Admin.Pages.Catalog.Products
{
    public partial class UpsertProduct : IAsyncDisposable
    {
        [Parameter]
        public string? Id { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

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
        private IToastService ToastService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private List<BreadcrumbItem> breadcrumb = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Label = "Home", Url = "/", IsActive = false },
            new BreadcrumbItem { Label = "Products", Url = "/products", IsActive = false }
        };

        private bool isEditMode => !string.IsNullOrEmpty(Id);

        private CreateProductFormModel createProductForm = new();

        private ProductFormModel productForm => createProductForm.Product;

        private List<BrandModel> brands = new();

        private List<BasicCategoryResponse> basicCategories = new();

        private List<ProductTemplateResponse> productTemplates = new();

        private List<ProductAttributeResponse> allAttributes = new();

        private string? selectedTemplateId;

        private string? selectedAvailableAttributeId;

        private List<ProductAttributeResponse> availableAttributes = new();

        private string? thumbnailPreviewUrl;

        private List<string> productImagePreviewUrls = new();

        private bool quillInitialized;

        private List<BasicProductResponse> allProducts = new();

        // Formatted price properties - bỏ trailing .00
        private string PriceFormatted
        {
            get => NumberFormatHelper.FormatDecimal(productForm.Price);
            set => productForm.Price = NumberFormatHelper.ParseDecimal(value);
        }

        private string OldPriceFormatted
        {
            get => NumberFormatHelper.FormatDecimal(productForm.OldPrice);
            set => productForm.OldPrice = NumberFormatHelper.ParseDecimal(value);
        }

        private string SpecialPriceFormatted
        {
            get => NumberFormatHelper.FormatDecimal(productForm.SpecialPrice);
            set => productForm.SpecialPrice = NumberFormatHelper.ParseDecimal(value);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender || (!quillInitialized && productForm != null))
            {
                await JSRuntime.InvokeVoidAsync("externalLibs.createQuill", "#short-description-editor", productForm.ShortDescription);
                await JSRuntime.InvokeVoidAsync("externalLibs.createQuill", "#description-editor", productForm.Description);
                quillInitialized = true;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            breadcrumb.Add(new BreadcrumbItem
            {
                Label = isEditMode ? "Edit Product" : "Add Product",
                Url = isEditMode ? $"/products/edit/{Id}" : "/products/add",
                IsActive = true
            });

            await Task.WhenAll(
                GetBrands(),
                GetBasicCategories(),
                GetProductTemplates(),
                GetAllAttributes(),
                GetAllProducts()
            );

            if (isEditMode)
            {
                var productResponse = await ProductApi.GetProductAsync(Id);
                if (!productResponse.IsSuccess)
                {
                    ToastService.ShowError(productResponse.Message);
                    return;
                }

                var data = productResponse.Data;

                // Map data to productForm
                productForm.Id = data.Id;
                productForm.Name = data.Name;
                productForm.BrandId = data.BrandId;
                productForm.ShortDescription = data.ShortDescription;
                productForm.Description = data.Description;
                productForm.MetaTitle = data.MetaTitle;
                productForm.MetaKeywords = data.MetaKeywords;
                productForm.MetaDescription = data.MetaDescription;
                productForm.IsPublished = data.IsPublished;
                productForm.Price = data.Price;
                productForm.OldPrice = data.OldPrice;
                productForm.SpecialPrice = data.SpecialPrice;
                productForm.SpecialPriceStart = data.SpecialPriceStart;
                productForm.SpecialPriceEnd = data.SpecialPriceEnd;
                productForm.IsFeatured = data.IsFeatured;
                productForm.IsCallForPricing = data.IsCallForPricing;
                productForm.IsAllowToOrder = data.IsAllowToOrder;
                productForm.Sku = data.Sku;
                productForm.Gtin = data.Gtin;
                productForm.ThumbnailImageUrl = data.ThumbnailImageUrl;

                // Map CategoryIds
                productForm.CategoryIds.Clear();
                if (data.CategoryIds != null && data.CategoryIds.Any())
                {
                    foreach (var catId in data.CategoryIds)
                    {
                        productForm.CategoryIds.Add(catId);
                    }
                }

                // Map Attributes
                if (data.Attributes != null)
                {
                    productForm.Attributes = data.Attributes.Select(a => new ProductAttributeFormItem
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Value = a.Value,
                        GroupName = a.GroupName
                    }).ToList();
                    RefreshAvailableAttributes();
                }

                if (!string.IsNullOrEmpty(productForm.ThumbnailImageUrl))
                {
                    thumbnailPreviewUrl = productForm.ThumbnailImageUrl;
                }

                // Map existing Product Images
                if (data.ProductImages != null)
                {
                    productForm.ProductImages = data.ProductImages.Select(m => new ProductMediaFormItem
                    {
                        Id = m.Id,
                        Caption = m.Caption,
                        MediaUrl = m.MediaUrl
                    }).ToList();
                }

                // Map existing Product Documents
                if (data.ProductDocuments != null)
                {
                    productForm.ProductDocuments = data.ProductDocuments.Select(m => new ProductMediaFormItem
                    {
                        Id = m.Id,
                        Caption = m.Caption,
                        MediaUrl = m.MediaUrl
                    }).ToList();
                }

                // Map Related & Cross-sell Products
                if (data.RelatedProducts != null)
                {
                    productForm.RelatedProducts = data.RelatedProducts.Select(p => new ProductLinkFormItem
                    {
                        Id = p.Id,
                        Name = p.Name,
                        IsPublished = p.IsPublished
                    }).ToList();
                }

                if (data.CrossSellProducts != null)
                {
                    productForm.CrossSellProducts = data.CrossSellProducts.Select(p => new ProductLinkFormItem
                    {
                        Id = p.Id,
                        Name = p.Name,
                        IsPublished = p.IsPublished
                    }).ToList();
                }

                // Update Quill editors after data is loaded
                if (quillInitialized)
                {
                    await JSRuntime.InvokeVoidAsync("externalLibs.setQuillHtml", "#short-description-editor", productForm.ShortDescription ?? string.Empty);
                    await JSRuntime.InvokeVoidAsync("externalLibs.setQuillHtml", "#description-editor", productForm.Description ?? string.Empty);
                }
            }
        }

        private async Task GetBrands()
        {
            var response = await BrandApi.GetBrandsAsync(1, 100);
            if (response.IsSuccess)
            {
                brands = response.Data;
            }
        }

        private async Task GetBasicCategories()
        {
            var response = await CategoryApi.GetBasicCategoriesAsync();
            if (response.IsSuccess)
            {
                basicCategories = response.Data;
            }
        }

        private async Task GetProductTemplates()
        {
            var response = await ProductTemplateApi.GetProductTemplatesAsync(1, 100);
            if (response.IsSuccess)
            {
                productTemplates = response.Data;
            }
        }

        private async Task GetAllAttributes()
        {
            var response = await ProductAttributeApi.GetProductAttributesAsync(1, 100);
            if (response.IsSuccess)
            {
                allAttributes = response.Data;
                RefreshAvailableAttributes();
            }
        }

        private void RefreshAvailableAttributes()
        {
            var usedIds = productForm.Attributes.Select(a => a.Id).ToHashSet();
            availableAttributes = allAttributes.Where(a => !usedIds.Contains(a.Id)).ToList();
        }

        private void ApplyTemplate()
        {
            if (string.IsNullOrEmpty(selectedTemplateId))
                return;

            var template = productTemplates.FirstOrDefault(t => t.Id == selectedTemplateId);
            if (template == null)
                return;

            productForm.Attributes.Clear();

            foreach (var attr in template.ProductAttributes)
            {
                var fullAttr = allAttributes.FirstOrDefault(a => a.Id == attr.Id);
                productForm.Attributes.Add(new ProductAttributeFormItem
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
            if (string.IsNullOrEmpty(selectedAvailableAttributeId))
                return;

            var attr = allAttributes.FirstOrDefault(a => a.Id == selectedAvailableAttributeId);
            if (attr == null)
                return;

            if (productForm.Attributes.Any(a => a.Id == attr.Id))
                return;

            productForm.Attributes.Add(new ProductAttributeFormItem
            {
                Id = attr.Id,
                Name = attr.Name,
                Value = string.Empty,
                GroupName = attr.GroupName ?? string.Empty
            });

            selectedAvailableAttributeId = null;
            RefreshAvailableAttributes();
        }

        private void RemoveAttribute(string attributeId)
        {
            var item = productForm.Attributes.FirstOrDefault(a => a.Id == attributeId);
            if (item != null)
            {
                productForm.Attributes.Remove(item);
                RefreshAvailableAttributes();
            }
        }

        private void OnCategoryCheckedChanged(string categoryId, object checkedValue)
        {
            var isChecked = (bool)checkedValue;
            if (isChecked)
            {
                if (!productForm.CategoryIds.Contains(categoryId))
                {
                    productForm.CategoryIds.Add(categoryId);
                }
            }
            else
            {
                productForm.CategoryIds.Remove(categoryId);
            }
        }

        private async Task OnThumbnailSelected(InputFileChangeEventArgs e)
        {
            var file = e.File;

            if (file.Size > FileUploadConstants.MaxImageSize)
            {
                ToastService.ShowError(FileUploadConstants.ImageSizeExceededMessage);
                return;
            }

            if (!FileUploadConstants.AllowedImageTypes.Contains(file.ContentType))
            {
                ToastService.ShowError(FileUploadConstants.InvalidImageTypeMessage);
                return;
            }

            try
            {
                var buffer = new byte[file.Size];
                await file.OpenReadStream(FileUploadConstants.MaxImageSize).ReadAsync(buffer);
                var imageBase64 = Convert.ToBase64String(buffer);
                thumbnailPreviewUrl = $"data:{file.ContentType};base64,{imageBase64}";
                createProductForm.ThumbnailImage = file;
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Error loading image: {ex.Message}");
                createProductForm.ThumbnailImage = null;
                thumbnailPreviewUrl = null;
            }
        }

        private void RemoveThumbnail()
        {
            createProductForm.ThumbnailImage = null;
            thumbnailPreviewUrl = null;
            productForm.ThumbnailImageUrl = null;
        }

        private async Task OnProductImagesSelected(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles())
            {
                if (file.Size > FileUploadConstants.MaxImageSize)
                {
                    ToastService.ShowError($"{file.Name}: {FileUploadConstants.ImageSizeExceededMessage}");
                    continue;
                }

                if (!FileUploadConstants.AllowedImageTypes.Contains(file.ContentType))
                {
                    ToastService.ShowError($"{file.Name}: {FileUploadConstants.InvalidImageTypeMessage}");
                    continue;
                }

                try
                {
                    var buffer = new byte[file.Size];
                    await file.OpenReadStream(FileUploadConstants.MaxImageSize).ReadAsync(buffer);
                    var imageBase64 = Convert.ToBase64String(buffer);
                    var previewUrl = $"data:{file.ContentType};base64,{imageBase64}";

                    createProductForm.ProductImages.Add(file);
                    productImagePreviewUrls.Add(previewUrl);
                }
                catch (Exception ex)
                {
                    ToastService.ShowError($"Error loading {file.Name}: {ex.Message}");
                }
            }
        }

        private void RemoveProductImage(int index)
        {
            createProductForm.ProductImages.RemoveAt(index);
            productImagePreviewUrls.RemoveAt(index);
        }

        private void RemoveExistingProductImage(string mediaId)
        {
            var item = productForm.ProductImages.FirstOrDefault(x => x.Id == mediaId);
            if (item != null)
            {
                productForm.ProductImages.Remove(item);
                productForm.DeletedMediaIds.Add(mediaId);
            }
        }

        private void OnProductDocumentsSelected(InputFileChangeEventArgs e)
        {
            foreach (var file in e.GetMultipleFiles())
            {
                if (file.Size > FileUploadConstants.MaxDocumentSize)
                {
                    ToastService.ShowError($"{file.Name}: {FileUploadConstants.DocumentSizeExceededMessage}");
                    continue;
                }

                if (!FileUploadConstants.AllowedDocumentTypes.Contains(file.ContentType))
                {
                    ToastService.ShowError($"{file.Name}: {FileUploadConstants.InvalidDocumentTypeMessage}");
                    continue;
                }

                createProductForm.ProductDocuments.Add(file);
            }
        }

        private void RemoveProductDocument(int index)
        {
            createProductForm.ProductDocuments.RemoveAt(index);
        }

        private void RemoveExistingProductDocument(string mediaId)
        {
            var item = productForm.ProductDocuments.FirstOrDefault(x => x.Id == mediaId);
            if (item != null)
            {
                productForm.ProductDocuments.Remove(item);
                productForm.DeletedMediaIds.Add(mediaId);
            }
        }

        private async Task<MultipartFormDataContent> CreateMultipartFormDataContent()
        {
            var content = new MultipartFormDataContent();

            if (!string.IsNullOrEmpty(productForm.Id))
                content.Add(new StringContent(productForm.Id), "Product.Id");

            content.Add(new StringContent(productForm.Name ?? string.Empty), "Product.Name");
            content.Add(new StringContent(productForm.ShortDescription ?? string.Empty), "Product.ShortDescription");
            content.Add(new StringContent(productForm.Description ?? string.Empty), "Product.Description");
            content.Add(new StringContent(productForm.MetaTitle ?? string.Empty), "Product.MetaTitle");
            content.Add(new StringContent(productForm.MetaKeywords ?? string.Empty), "Product.MetaKeywords");
            content.Add(new StringContent(productForm.MetaDescription ?? string.Empty), "Product.MetaDescription");
            content.Add(new StringContent(productForm.Sku ?? string.Empty), "Product.Sku");
            content.Add(new StringContent(productForm.Gtin ?? string.Empty), "Product.Gtin");
            content.Add(new StringContent(productForm.Price.ToString()), "Product.Price");
            content.Add(new StringContent(productForm.IsPublished.ToString()), "Product.IsPublished");
            content.Add(new StringContent(productForm.IsFeatured.ToString()), "Product.IsFeatured");
            content.Add(new StringContent(productForm.IsCallForPricing.ToString()), "Product.IsCallForPricing");
            content.Add(new StringContent(productForm.IsAllowToOrder.ToString()), "Product.IsAllowToOrder");

            if (productForm.OldPrice.HasValue)
                content.Add(new StringContent(productForm.OldPrice.Value.ToString()), "Product.OldPrice");

            if (productForm.SpecialPrice.HasValue)
                content.Add(new StringContent(productForm.SpecialPrice.Value.ToString()), "Product.SpecialPrice");

            if (productForm.SpecialPriceStart.HasValue)
                content.Add(new StringContent(productForm.SpecialPriceStart.Value.ToString("o")), "Product.SpecialPriceStart");

            if (productForm.SpecialPriceEnd.HasValue)
                content.Add(new StringContent(productForm.SpecialPriceEnd.Value.ToString("o")), "Product.SpecialPriceEnd");

            if (!string.IsNullOrEmpty(productForm.BrandId))
                content.Add(new StringContent(productForm.BrandId), "Product.BrandId");

            for (var i = 0; i < productForm.CategoryIds.Count; i++)
            {
                content.Add(new StringContent(productForm.CategoryIds[i]), $"Product.CategoryIds[{i}]");
            }

            for (var i = 0; i < productForm.DeletedMediaIds.Count; i++)
            {
                content.Add(new StringContent(productForm.DeletedMediaIds[i]), $"Product.DeletedMediaIds[{i}]");
            }

            for (var i = 0; i < productForm.Attributes.Count; i++)
            {
                content.Add(new StringContent(productForm.Attributes[i].Id), $"Product.Attributes[{i}].Id");
                content.Add(new StringContent(productForm.Attributes[i].Name ?? string.Empty), $"Product.Attributes[{i}].Name");
                content.Add(new StringContent(productForm.Attributes[i].Value ?? string.Empty), $"Product.Attributes[{i}].Value");
                content.Add(new StringContent(productForm.Attributes[i].GroupName ?? string.Empty), $"Product.Attributes[{i}].GroupName");
            }

            for (var i = 0; i < productForm.RelatedProducts.Count; i++)
            {
                content.Add(new StringContent(productForm.RelatedProducts[i].Id), $"Product.RelatedProducts[{i}].Id");
                content.Add(new StringContent(productForm.RelatedProducts[i].Name ?? string.Empty), $"Product.RelatedProducts[{i}].Name");
                content.Add(new StringContent(productForm.RelatedProducts[i].IsPublished.ToString()), $"Product.RelatedProducts[{i}].IsPublished");
            }

            for (var i = 0; i < productForm.CrossSellProducts.Count; i++)
            {
                content.Add(new StringContent(productForm.CrossSellProducts[i].Id), $"Product.CrossSellProducts[{i}].Id");
                content.Add(new StringContent(productForm.CrossSellProducts[i].Name ?? string.Empty), $"Product.CrossSellProducts[{i}].Name");
                content.Add(new StringContent(productForm.CrossSellProducts[i].IsPublished.ToString()), $"Product.CrossSellProducts[{i}].IsPublished");
            }

            // Thumbnail
            if (createProductForm.ThumbnailImage != null)
            {
                var fileContent = new StreamContent(createProductForm.ThumbnailImage.OpenReadStream(FileUploadConstants.MaxImageSize));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(createProductForm.ThumbnailImage.ContentType);
                content.Add(fileContent, "ThumbnailImage", createProductForm.ThumbnailImage.Name);
            }

            // Product images
            foreach (var imageFile in createProductForm.ProductImages)
            {
                var fileContent = new StreamContent(imageFile.OpenReadStream(FileUploadConstants.MaxImageSize));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
                content.Add(fileContent, "ProductImages", imageFile.Name);
            }

            // Product documents
            foreach (var docFile in createProductForm.ProductDocuments)
            {
                var fileContent = new StreamContent(docFile.OpenReadStream(FileUploadConstants.MaxDocumentSize));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(docFile.ContentType);
                content.Add(fileContent, "ProductDocuments", docFile.Name);
            }

            return content;
        }

        private async Task FormSubmitted()
        {
            productForm.ShortDescription = await JSRuntime.InvokeAsync<string>("externalLibs.getQuillHtml", "#short-description-editor");
            productForm.Description = await JSRuntime.InvokeAsync<string>("externalLibs.getQuillHtml", "#description-editor");

            var content = await CreateMultipartFormDataContent();

            BaseResponse<ProductFormModel> response;

            if (isEditMode)
            {
                response = await ProductApi.UpdateProductAsync(content);
                if (response.IsSuccess)
                {
                    NavigationManager.NavigateTo("/products");
                    ToastService.ShowSuccess(SystemMessages.UpdateDataSuccess);
                }
                else
                {
                    ToastService.ShowError(response.Message);
                }
            }
            else
            {
                response = await ProductApi.CreateProductAsync(content);
                if (response.IsSuccess)
                {
                    NavigationManager.NavigateTo("/products");
                    ToastService.ShowSuccess(SystemMessages.AddDataSuccess);
                }
                else
                {
                    ToastService.ShowError(response.Message);
                }
            }
        }

        private async Task GetAllProducts()
        {
            var response = await ProductApi.GetProductsAsync(1, 1000);
            if (response.IsSuccess)
            {
                allProducts = response.Data;
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (quillInitialized)
            {
                await JSRuntime.InvokeVoidAsync("externalLibs.destroyQuill", "#short-description-editor");
                await JSRuntime.InvokeVoidAsync("externalLibs.destroyQuill", "#description-editor");
            }
        }
    }
}
