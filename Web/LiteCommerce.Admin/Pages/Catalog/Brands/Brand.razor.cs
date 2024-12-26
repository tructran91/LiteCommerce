using LiteCommerce.Admin.Models;
using LiteCommerce.Admin.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LiteCommerce.Admin.Pages.Catalog.Brands
{
    public partial class Brand
    {
        [Inject]
        private IBrandService BrandService { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        private List<BreadcrumbItemModel> breadcrumb = new List<BreadcrumbItemModel>
        {
            new BreadcrumbItemModel { Label = "Home", Url = "/", IsActive = false },
            new BreadcrumbItemModel { Label = "Brands", Url = "/brands", IsActive = true }
        };

        private List<BrandModel> brands = new();

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
