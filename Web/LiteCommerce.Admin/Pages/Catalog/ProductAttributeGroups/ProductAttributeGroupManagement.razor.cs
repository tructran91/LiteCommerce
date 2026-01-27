using Blazored.Toast.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Application;
using LiteCommerce.Admin.Models.Business.ProductAttributeGroup;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LiteCommerce.Admin.Pages.Catalog.ProductAttributeGroups
{
    public partial class ProductAttributeGroupManagement
    {
        [Inject]
        private IProductAttributeGroupApi ProductAttributeGroupApi { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public IToastService ToastService { get; set; }

        [Inject]
        public SweetAlertService SweetAlertService { get; set; }

        private List<BreadcrumbItem> breadcrumb = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Label = "Home", Url = "/", IsActive = false },
            new BreadcrumbItem { Label = "Product Attribute Groups", Url = "/product-attribute-groups", IsActive = true }
        };

        private List<ProductAttributeGroupResponse> productAttributeGroups = new();

        private ProductAttributeGroupFormModel productAttributeGroupForm = new ProductAttributeGroupFormModel();

        private bool isLoading = true;

        private bool isEditMode = false;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await GetProductAttributeGroups();
        }

        private async Task ShowModal()
        {
            await JSRuntime.InvokeVoidAsync("externalLibs.showModal", "productAttributeGroupModal");
        }

        private async Task HideModal()
        {
            await JSRuntime.InvokeVoidAsync("externalLibs.hideModal", "productAttributeGroupModal");
        }

        private async Task GetProductAttributeGroups()
        {
            var response = await ProductAttributeGroupApi.GetProductAttributeGroupsAsync(1, 100);
            if (response.IsSuccess)
            {
                productAttributeGroups = response.Data;
            }
            isLoading = false;
        }

        private async Task OpenAddModal()
        {
            isEditMode = false;
            productAttributeGroupForm = new ProductAttributeGroupFormModel();
            await ShowModal();
        }

        private async Task OpenEditModal(string productAttributeGroupId)
        {
            isEditMode = true;
            var request = await ProductAttributeGroupApi.GetProductAttributeGroupAsync(productAttributeGroupId);
            if (request.IsSuccess)
            {
                var productAttributeGroupToEdit = request.Data;
                productAttributeGroupForm = new ProductAttributeGroupFormModel
                {
                    Id = productAttributeGroupToEdit.Id,
                    Name = productAttributeGroupToEdit.Name,
                };
                await ShowModal();
            }
            else
            {
                ToastService.ShowError(request.Message);
            }
        }

        private async Task OpenDeleteModal(string productAttributeGroupId, string productAttributeGroupName)
        {
            var confirmationResult = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirm Attribute Group Deletion",
                Html = $"Are you sure you want to delete the attribute group <strong>{productAttributeGroupName}</strong>? This action cannot be undone.",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Yes, delete it!",
                CancelButtonText = "Cancel"
            });

            if (!confirmationResult.IsConfirmed)
            {
                return;
            }

            var deletedResult = await ProductAttributeGroupApi.DeleteProductAttributeGroupAsync(productAttributeGroupId);

            if (deletedResult.IsSuccess)
            {
                await GetProductAttributeGroups();
            }
            else
            {
                ToastService.ShowError(deletedResult.Message);
            }
        }

        private async Task FormSubmitted()
        {
            var response = isEditMode
                ? await ProductAttributeGroupApi.UpdateProductAttributeGroupAsync(productAttributeGroupForm)
                : await ProductAttributeGroupApi.CreateProductAttributeGroupAsync(productAttributeGroupForm);

            if (response.IsSuccess)
            {
                await GetProductAttributeGroups();
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
