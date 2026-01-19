using Blazored.Toast.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Models.Application;
using LiteCommerce.Admin.Models.Business.Category;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LiteCommerce.Admin.Pages.Catalog.Categories
{
    public partial class CategoryManagement
    {
        [Inject]
        private ICategoryApi CategoryApi { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public IToastService ToastService { get; set; }

        [Inject]
        public SweetAlertService SweetAlertService { get; set; }

        private List<BreadcrumbItem> breadcrumb = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Label = "Home", Url = "/", IsActive = false },
            new BreadcrumbItem { Label = "Categories", Url = "/categories", IsActive = true }
        };

        private List<CategoryResponse> categories = new();

        private bool isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GetCategories();
        }

        private async Task GetCategories()
        {
            var response = await CategoryApi.GetCategoriesAsync(1, 100);
            if (response.IsSuccess)
            {
                categories = response.Data;
            }
            isLoading = false;
        }
    }
}
