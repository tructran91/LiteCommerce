using Blazored.Toast.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Application;
using LiteCommerce.Admin.Models.Business.ProductAttributeGroup;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LiteCommerce.Admin.Pages.Catalog.ProductAttributes
{
    public partial class ProductAttributeManagement
    {
        [Inject]
        private IProductAttributeApi ProductAttributeApi { get; set; }

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
            new BreadcrumbItem { Label = "Product Attributes", Url = "/product-attributes", IsActive = true }
        };

        private List<ProductAttributeResponse> productAttributes = new();

        private List<ProductAttributeGroupResponse> productAttributeGroups = new();

        private ProductAttributeFormModel productAttributeForm = new ProductAttributeFormModel();

        private bool isLoading = true;

        private bool isEditMode = false;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await Task.WhenAll(GetProductAttributes(), GetProductAttributeGroups());
        }

        private async Task ShowModal()
        {
            await JSRuntime.InvokeVoidAsync("externalLibs.showModal", "productAttributeModal");
        }

        private async Task HideModal()
        {
            await JSRuntime.InvokeVoidAsync("externalLibs.hideModal", "productAttributeModal");
        }

        private async Task GetProductAttributes()
        {
            var response = await ProductAttributeApi.GetProductAttributesAsync(1, 100);
            if (response.IsSuccess)
            {
                productAttributes = response.Data;
            }
            isLoading = false;
        }

        private async Task GetProductAttributeGroups()
        {
            var response = await ProductAttributeGroupApi.GetProductAttributeGroupsAsync(1, 100);
            if (response.IsSuccess)
            {
                productAttributeGroups = response.Data;
            }
        }

        private string GetGroupNameById(string groupId)
        {
            var group = productAttributeGroups.FirstOrDefault(g => g.Id == groupId);
            return group?.Name ?? "N/A";
        }

        private async Task OpenAddModal()
        {
            isEditMode = false;
            productAttributeForm = new ProductAttributeFormModel();
            await ShowModal();
        }

        private async Task OpenEditModal(string productAttributeId)
        {
            isEditMode = true;
            var request = await ProductAttributeApi.GetProductAttributeAsync(productAttributeId);
            if (request.IsSuccess)
            {
                var productAttributeToEdit = request.Data;
                productAttributeForm = new ProductAttributeFormModel
                {
                    Id = productAttributeToEdit.Id,
                    Name = productAttributeToEdit.Name,
                    GroupId = productAttributeToEdit.GroupId,
                };
                await ShowModal();
            }
            else
            {
                ToastService.ShowError(request.Message);
            }
        }

        private async Task OpenDeleteModal(string productAttributeId, string productAttributeName)
        {
            var confirmationResult = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirm Attribute Deletion",
                Html = $"Are you sure you want to delete the attribute <strong>{productAttributeName}</strong>? This action cannot be undone.",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Yes, delete it!",
                CancelButtonText = "Cancel"
            });

            if (!confirmationResult.IsConfirmed)
            {
                return;
            }

            var deletedResult = await ProductAttributeApi.DeleteProductAttributeAsync(productAttributeId);

            if (deletedResult.IsSuccess)
            {
                await GetProductAttributes();
            }
            else
            {
                ToastService.ShowError(deletedResult.Message);
            }
        }

        private async Task FormSubmitted()
        {
            var response = isEditMode
                ? await ProductAttributeApi.UpdateProductAttributeAsync(productAttributeForm)
                : await ProductAttributeApi.CreateProductAttributeAsync(productAttributeForm);

            if (response.IsSuccess)
            {
                await GetProductAttributes();
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
