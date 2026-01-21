using Blazored.Toast.Services;
using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Application;
using LiteCommerce.Admin.Models.Business.Category;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace LiteCommerce.Admin.Pages.Catalog.Categories
{
    public partial class UpsertCategory
    {
        [Parameter]
        public string? Id { get; set; }

        private bool IsEditMode => !string.IsNullOrEmpty(Id);

        [Inject]
        private ICategoryApi CategoryApi { get; set; }

        [Inject]
        private IToastService ToastService { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private List<BreadcrumbItem> breadcrumb = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Label = "Home", Url = "/", IsActive = false },
            new BreadcrumbItem { Label = "Categories", Url = "/categories", IsActive = false }
        };

        private List<BasicCategoryResponse> basicCategories = new();

        private CategoryFormModel categoryForm = new();

        private IBrowserFile? thumbnailFile;
        private string? thumbnailPreviewUrl;
        private const long MaxFileSize = 5 * 1024 * 1024; // 5MB

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GetBasicCategories();

            // Update breadcrumb based on mode
            breadcrumb.Add(new BreadcrumbItem
            {
                Label = IsEditMode ? "Edit Category" : "Add Category",
                Url = IsEditMode ? $"/categories/edit/{Id}" : "/categories/add",
                IsActive = true
            });

            if (IsEditMode)
            {
                var categoryResponse = await CategoryApi.GetCategoryAsync(Id);
                if (!categoryResponse.IsSuccess)
                {
                    ToastService.ShowError(categoryResponse.Message);
                }
                categoryForm = categoryResponse.Data;
                
                // Load existing thumbnail if available
                if (!string.IsNullOrEmpty(categoryForm.ThumbnailUrl))
                {
                    thumbnailPreviewUrl = categoryForm.ThumbnailUrl;
                }
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

        private async Task OnThumbnailSelected(InputFileChangeEventArgs e)
        {
            thumbnailFile = e.File;

            if (thumbnailFile != null)
            {
                // Validate file size
                if (thumbnailFile.Size > MaxFileSize)
                {
                    ToastService.ShowError($"File size must not exceed {MaxFileSize / 1024 / 1024}MB");
                    thumbnailFile = null;
                    return;
                }

                // Validate file type
                if (!thumbnailFile.ContentType.StartsWith("image/"))
                {
                    ToastService.ShowError("Only image files are allowed");
                    thumbnailFile = null;
                    return;
                }

                // Create preview URL
                try
                {
                    var buffer = new byte[thumbnailFile.Size];
                    await thumbnailFile.OpenReadStream(MaxFileSize).ReadAsync(buffer);
                    var imageBase64 = Convert.ToBase64String(buffer);
                    thumbnailPreviewUrl = $"data:{thumbnailFile.ContentType};base64,{imageBase64}";
                }
                catch (Exception ex)
                {
                    ToastService.ShowError($"Error loading image: {ex.Message}");
                    thumbnailFile = null;
                    thumbnailPreviewUrl = null;
                }
            }
        }

        private void RemoveThumbnail()
        {
            thumbnailFile = null;
            thumbnailPreviewUrl = null;
        }

        private async Task FormSubmitted()
        {
            // TODO: Upload thumbnail to server if thumbnailFile is not null
            // You'll need to add an API endpoint to handle file upload
            // Example:
            // if (thumbnailFile != null)
            // {
            //     var uploadResponse = await CategoryApi.UploadThumbnailAsync(thumbnailFile);
            //     if (uploadResponse.IsSuccess)
            //     {
            //         categoryForm.ThumbnailUrl = uploadResponse.Data;
            //     }
            // }

            var response = IsEditMode
                ? await CategoryApi.UpdateCategoryAsync(categoryForm)
                : await CategoryApi.CreateCategoryAsync(categoryForm);

            if (response.IsSuccess)
            {
                NavigationManager.NavigateTo("/categories");

                var successMessage = IsEditMode
                    ? SystemMessages.UpdateDataSuccess
                    : SystemMessages.AddDataSuccess;
                ToastService.ShowSuccess(successMessage);
            }
            else
            {
                ToastService.ShowError(response.Message);
            }
        }
    }
}
