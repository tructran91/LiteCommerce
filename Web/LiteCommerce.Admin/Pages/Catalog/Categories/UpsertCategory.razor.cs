using Blazored.Toast.Services;
using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Application;
using LiteCommerce.Admin.Models.Business.Category;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Headers;

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
                if (!string.IsNullOrEmpty(categoryForm.ThumbnailImageUrl))
                {
                    // Convert physical path or relative path to proper URL
                    thumbnailPreviewUrl = categoryForm.ThumbnailImageUrl;
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
            categoryForm.ThumbnailImageUrl = null;
        }

        private async Task<MultipartFormDataContent> CreateMultipartFormDataContent()
        {
            var content = new MultipartFormDataContent();

            // Add all form fields
            if (!string.IsNullOrEmpty(categoryForm.Id))
                content.Add(new StringContent(categoryForm.Id), "Id");

            content.Add(new StringContent(categoryForm.Name ?? string.Empty), "Name");
            content.Add(new StringContent(categoryForm.Slug ?? string.Empty), "Slug");
            content.Add(new StringContent(categoryForm.Description ?? string.Empty), "Description");
            content.Add(new StringContent(categoryForm.MetaTitle ?? string.Empty), "MetaTitle");
            content.Add(new StringContent(categoryForm.MetaKeywords ?? string.Empty), "MetaKeywords");
            content.Add(new StringContent(categoryForm.MetaDescription ?? string.Empty), "MetaDescription");
            content.Add(new StringContent(categoryForm.IsPublished.ToString()), "IsPublished");
            content.Add(new StringContent(categoryForm.IncludeInMenu.ToString()), "IncludeInMenu");
            content.Add(new StringContent(categoryForm.DisplayOrder.ToString()), "DisplayOrder");

            if (!string.IsNullOrEmpty(categoryForm.ParentId))
                content.Add(new StringContent(categoryForm.ParentId), "ParentId");

            // Add thumbnail file if selected
            if (thumbnailFile != null)
            {
                var fileContent = new StreamContent(thumbnailFile.OpenReadStream(MaxFileSize));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(thumbnailFile.ContentType);
                content.Add(fileContent, "ThumbnailImage", thumbnailFile.Name);
            }

            return content;
        }

        private async Task FormSubmitted()
        {
            var content = await CreateMultipartFormDataContent();

            var response = IsEditMode
                ? await CategoryApi.UpdateCategoryAsync(content)
                : await CategoryApi.CreateCategoryAsync(content);

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
