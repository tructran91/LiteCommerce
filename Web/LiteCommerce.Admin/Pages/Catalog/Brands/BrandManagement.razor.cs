using Blazored.Toast.Services;
using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Models.Application;
using LiteCommerce.Admin.Models.Business;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LiteCommerce.Admin.Pages.Catalog.Brands
{
    public partial class BrandManagement
    {
        [Inject]
        private IBrandApi BrandApi { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public IToastService ToastService { get; set; }

        private List<BreadcrumbItem> breadcrumb = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Label = "Home", Url = "/", IsActive = false },
            new BreadcrumbItem { Label = "Brands", Url = "/brands", IsActive = true }
        };

        private List<Brand> brands = new();

        private Brand addEditBrand = new Brand();

        private bool isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GetBrands();
        }

        private async Task ShowModal()
        {
            await JSRuntime.InvokeVoidAsync("externalLibs.showModal", "addBrandModal");
        }

        private async Task HideModal()
        {
            await JSRuntime.InvokeVoidAsync("externalLibs.hideModal", "addBrandModal");
        }

        private async Task GetBrands()
        {
            var response = await BrandApi.GetBrandsAsync();
            brands = response.Data;
            isLoading = false;
        }

        private async Task FormSubmitted()
        {
            ToastService.ShowSuccess("thanh cong");
            await JSRuntime.InvokeVoidAsync("externalLibs.hideModal", "addBrandModal");
            //var request = await BrandApi.CreateBrandAsync(addEditBrand);
        }
    }
}
