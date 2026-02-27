using Blazored.Toast.Services;
using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Application;
using LiteCommerce.Admin.Models.Business.Brand;
using LiteCommerce.Admin.Models.Business.Category;
using LiteCommerce.Admin.Models.Business.Product;
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

        private string? thumbnailPreviewUrl;

        private List<string> productImagePreviewUrls = new();

        private bool quillInitialized;

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

            await GetBrands();
            await GetBasicCategories();

            if (isEditMode)
            {
                var productResponse = await ProductApi.GetProductAsync(Id);
                if (!productResponse.IsSuccess)
                {
                    ToastService.ShowError(productResponse.Message);
                }
                createProductForm.Product = productResponse.Data;

                if (!string.IsNullOrEmpty(productForm.ThumbnailImageUrl))
                {
                    thumbnailPreviewUrl = productForm.ThumbnailImageUrl;
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

            // Debug: đặt breakpoint ở dòng dưới, hover vào createProductForm để xem toàn bộ data
            var debugModel = new
            {
                createProductForm.Product,
                ThumbnailImageName = createProductForm.ThumbnailImage?.Name,
                ThumbnailImageSize = createProductForm.ThumbnailImage?.Size,
                ProductImages = createProductForm.ProductImages.Select(f => new { f.Name, f.Size }).ToList(),
                ProductDocuments = createProductForm.ProductDocuments.Select(f => new { f.Name, f.Size }).ToList()
            };

            var content = await CreateMultipartFormDataContent();

            //var response = await ProductApi.CreateProductAsync(content);

            //if (response.IsSuccess)
            //{
            //    NavigationManager.NavigateTo("/products");
            //    ToastService.ShowSuccess(SystemMessages.AddDataSuccess);
            //}
            //else
            //{
            //    ToastService.ShowError(response.Message);
            //}
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
