using Blazored.Toast.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Models.Application;
using LiteCommerce.Admin.Models.Business.Product;
using Microsoft.AspNetCore.Components;

namespace LiteCommerce.Admin.Pages.Catalog.Products
{
    public partial class ProductManagement
    {
        [Inject]
        private IProductApi ProductApi { get; set; }

        [Inject]
        public IToastService ToastService { get; set; }

        [Inject]
        public SweetAlertService SweetAlertService { get; set; }

        private List<BreadcrumbItem> breadcrumb = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Label = "Home", Url = "/", IsActive = false },
            new BreadcrumbItem { Label = "Products", Url = "/products", IsActive = true }
        };

        private List<BasicProductResponse> products = new();

        private bool isLoading = true;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GetProducts();
        }

        private async Task GetProducts()
        {
            var response = await ProductApi.GetProductsAsync(1, 100);
            if (response.IsSuccess)
            {
                products = response.Data;
            }
            isLoading = false;
        }

        private async Task OpenDeleteModal(string productId, string productName)
        {
            var confirmationResult = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirm Product Deletion",
                Html = $"Are you sure you want to delete the product <strong>{productName}</strong>? This action cannot be undone.",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Yes, delete it!",
                CancelButtonText = "Cancel"
            });

            if (!confirmationResult.IsConfirmed)
            {
                return;
            }

            var deletedResult = await ProductApi.DeleteProductAsync(productId);

            if (deletedResult.IsSuccess)
            {
                await GetProducts();
                ToastService.ShowSuccess("Product deleted successfully!");
            }
            else
            {
                ToastService.ShowError(deletedResult.Message);
            }
        }
    }
}
