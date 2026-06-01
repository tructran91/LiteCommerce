using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Category;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Net.Http.Headers;

namespace LiteCommerce.Admin.Pages.Catalog.Categories
{
    public partial class UpsertCategory
    {
        [Parameter]
        public string? Id { get; set; }

        [Inject]
        private ICategoryApi CategoryApi { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private List<BreadcrumbItem> _breadcrumbs = new();
        private bool _isEditMode => !string.IsNullOrEmpty(Id);
        private List<BasicCategoryResponse> _basicCategories = new();
        private CategoryFormModel _categoryForm = new();
        private IBrowserFile? _thumbnailFile;
        private string? _thumbnailPreviewUrl;
        private bool _loading = false;
        private MudForm _form = null!;

        protected override async Task OnInitializedAsync()
        {
            _breadcrumbs = new()
            {
                new("Home", href: "/", icon: Icons.Material.Filled.Home),
                new("Catalog", href: null, disabled: true),
                new("Categories", href: "/categories"),
                new(_isEditMode ? "Edit Category" : "Add Category", href: null, disabled: true)
            };

            await LoadBasicCategories();

            if (_isEditMode)
            {
                await LoadCategory();
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

        private async Task LoadCategory()
        {
            _loading = true;
            var response = await CategoryApi.GetCategoryAsync(Id!);

            if (response.IsSuccess)
            {
                _categoryForm = response.Data;

                if (!string.IsNullOrEmpty(_categoryForm.ThumbnailImageUrl))
                {
                    _thumbnailPreviewUrl = _categoryForm.ThumbnailImageUrl;
                }
            }
            else
            {
                Snackbar.Add(response.Message ?? SystemMessages.ErrorOccurred, Severity.Error);
            }

            _loading = false;
        }

        private async Task OnThumbnailSelected(InputFileChangeEventArgs e)
        {
            _thumbnailFile = e.File;

            if (_thumbnailFile != null)
            {
                if (_thumbnailFile.Size > FileUploadConstants.MaxImageSize)
                {
                    Snackbar.Add(FileUploadConstants.ImageSizeExceededMessage, Severity.Error);
                    _thumbnailFile = null;
                    return;
                }

                if (!FileUploadConstants.AllowedImageTypes.Contains(_thumbnailFile.ContentType))
                {
                    Snackbar.Add(FileUploadConstants.InvalidImageTypeMessage, Severity.Error);
                    _thumbnailFile = null;
                    return;
                }

                try
                {
                    var buffer = new byte[_thumbnailFile.Size];
                    await _thumbnailFile.OpenReadStream(FileUploadConstants.MaxImageSize).ReadAsync(buffer);
                    var imageBase64 = Convert.ToBase64String(buffer);
                    _thumbnailPreviewUrl = $"data:{_thumbnailFile.ContentType};base64,{imageBase64}";
                }
                catch (Exception ex)
                {
                    Snackbar.Add($"Error loading image: {ex.Message}", Severity.Error);
                    _thumbnailFile = null;
                    _thumbnailPreviewUrl = null;
                }
            }
        }

        private void RemoveThumbnail()
        {
            _thumbnailFile = null;
            _thumbnailPreviewUrl = null;
            _categoryForm.ThumbnailImageUrl = null;
        }

        private async Task<MultipartFormDataContent> CreateMultipartFormDataContent()
        {
            var content = new MultipartFormDataContent();

            if (!string.IsNullOrEmpty(_categoryForm.Id))
                content.Add(new StringContent(_categoryForm.Id), "Id");

            content.Add(new StringContent(_categoryForm.Name ?? string.Empty), "Name");
            content.Add(new StringContent(_categoryForm.Slug ?? string.Empty), "Slug");
            content.Add(new StringContent(_categoryForm.Description ?? string.Empty), "Description");
            content.Add(new StringContent(_categoryForm.MetaTitle ?? string.Empty), "MetaTitle");
            content.Add(new StringContent(_categoryForm.MetaKeywords ?? string.Empty), "MetaKeywords");
            content.Add(new StringContent(_categoryForm.MetaDescription ?? string.Empty), "MetaDescription");
            content.Add(new StringContent(_categoryForm.IsPublished.ToString()), "IsPublished");
            content.Add(new StringContent(_categoryForm.IncludeInMenu.ToString()), "IncludeInMenu");
            content.Add(new StringContent(_categoryForm.DisplayOrder.ToString()), "DisplayOrder");

            if (!string.IsNullOrEmpty(_categoryForm.ParentId))
                content.Add(new StringContent(_categoryForm.ParentId), "ParentId");

            if (_thumbnailFile != null)
            {
                var fileContent = new StreamContent(_thumbnailFile.OpenReadStream(FileUploadConstants.MaxImageSize));
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(_thumbnailFile.ContentType);
                content.Add(fileContent, "ThumbnailImage", _thumbnailFile.Name);
            }

            return content;
        }

        private async Task FormSubmitted()
        {
            await _form.Validate();
            if (!_form.IsValid) return;

            _loading = true;
            var content = await CreateMultipartFormDataContent();

            var response = _isEditMode
                ? await CategoryApi.UpdateCategoryAsync(content)
                : await CategoryApi.CreateCategoryAsync(content);

            if (response.IsSuccess)
            {
                var successMessage = _isEditMode
                    ? SystemMessages.UpdateDataSuccess
                    : SystemMessages.AddDataSuccess;

                Snackbar.Add(successMessage, Severity.Success);
                NavigationManager.NavigateTo("/categories");
            }
            else
            {
                Snackbar.Add(response.Message ?? SystemMessages.ErrorOccurred, Severity.Error);
            }

            _loading = false;
        }

        private void Cancel()
        {
            NavigationManager.NavigateTo("/categories");
        }
    }
}
