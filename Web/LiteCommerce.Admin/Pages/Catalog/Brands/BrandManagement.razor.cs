using LiteCommerce.Admin.Models.Application;
using LiteCommerce.Admin.Models.Business;
using LiteCommerce.Admin.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LiteCommerce.Admin.Pages.Catalog.Brands
{
    public partial class BrandManagement
    {
        [Inject]
        private IBrandService BrandService { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        private List<BreadcrumbItem> breadcrumb = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Label = "Home", Url = "/", IsActive = false },
            new BreadcrumbItem { Label = "Brands", Url = "/brands", IsActive = true }
        };

        private List<Brand> brands = new();

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            brands = await BrandService.GetBrands();
        }

        private async Task ShowModal()
        {
            await JSRuntime.InvokeVoidAsync("externalLibs.showModal", "addBrandModal");
        }

        private async Task HideModal()
        {
            await JSRuntime.InvokeVoidAsync("externalLibs.hideModal", "addBrandModal");
        }
    }
}
