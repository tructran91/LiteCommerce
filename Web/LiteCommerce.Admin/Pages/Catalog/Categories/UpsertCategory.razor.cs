using Blazored.Toast.Services;
using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Application;
using LiteCommerce.Admin.Models.Business.Category;
using Microsoft.AspNetCore.Components;

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

        private async Task FormSubmitted()
        {
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
