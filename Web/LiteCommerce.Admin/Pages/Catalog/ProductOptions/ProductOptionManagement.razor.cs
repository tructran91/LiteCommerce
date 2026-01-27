using Blazored.Toast.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Application;
using LiteCommerce.Admin.Models.Business.ProductOption;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LiteCommerce.Admin.Pages.Catalog.ProductOptions
{
    public partial class ProductOptionManagement
    {
        [Inject]
        private IProductOptionApi ProductOptionApi { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public IToastService ToastService { get; set; }

        [Inject]
        public SweetAlertService SweetAlertService { get; set; }

        private List<BreadcrumbItem> breadcrumb = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Label = "Home", Url = "/", IsActive = false },
            new BreadcrumbItem { Label = "Product Options", Url = "/product-options", IsActive = true }
        };

        private List<ProductOptionResponse> productOptions = new();

        private ProductOptionFormModel productOptionForm = new ProductOptionFormModel();

        private bool isLoading = true;

        private bool isEditMode = false;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GetProductOptions();
        }

        private async Task ShowModal()
        {
            await JSRuntime.InvokeVoidAsync("externalLibs.showModal", "productOptionModal");
        }

        private async Task HideModal()
        {
            await JSRuntime.InvokeVoidAsync("externalLibs.hideModal", "productOptionModal");
        }

        private async Task GetProductOptions()
        {
            var response = await ProductOptionApi.GetProductOptionsAsync(1, 100);
            if (response.IsSuccess)
            {
                productOptions = response.Data;
            }
            isLoading = false;
        }

        private async Task OpenAddModal()
        {
            isEditMode = false;
            productOptionForm = new ProductOptionFormModel();
            await ShowModal();
        }

        private async Task OpenEditModal(string productOptionId)
        {
            isEditMode = true;
            var request = await ProductOptionApi.GetProductOptionAsync(productOptionId);
            if (request.IsSuccess)
            {
                var productOptionToEdit = request.Data;
                productOptionForm = new ProductOptionFormModel
                {
                    Id = productOptionToEdit.Id,
                    Name = productOptionToEdit.Name,
                };
                await ShowModal();
            }
            else
            {
                ToastService.ShowError(request.Message);
            }
        }

        private async Task OpenDeleteModal(string productOptionId, string productOptionName)
        {
            var confirmationResult = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirm Product Option Deletion",
                Html = $"Are you sure you want to delete the product option <strong>{productOptionName}</strong>? This action cannot be undone.",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Yes, delete it!",
                CancelButtonText = "Cancel"
            });

            if (!confirmationResult.IsConfirmed)
            {
                return;
            }

            var deletedResult = await ProductOptionApi.DeleteProductOptionAsync(productOptionId);

            if (deletedResult.IsSuccess)
            {
                await GetProductOptions();
            }
            else
            {
                ToastService.ShowError(deletedResult.Message);
            }
        }

        private async Task FormSubmitted()
        {
            var response = isEditMode
                ? await ProductOptionApi.UpdateProductOptionAsync(productOptionForm)
                : await ProductOptionApi.CreateProductOptionAsync(productOptionForm);

            if (response.IsSuccess)
            {
                await GetProductOptions();
                await HideModal();

                var successMessage = isEditMode
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
