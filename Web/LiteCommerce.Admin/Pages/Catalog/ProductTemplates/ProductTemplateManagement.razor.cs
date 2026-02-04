using Blazored.Toast.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Application;
using LiteCommerce.Admin.Models.Business.ProductAttributeGroup;
using LiteCommerce.Admin.Models.Business.ProductTemplate;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LiteCommerce.Admin.Pages.Catalog.ProductTemplates
{
    public partial class ProductTemplateManagement
    {
        [Inject]
        private IProductTemplateApi ProductTemplateApi { get; set; }

        [Inject]
        private IProductAttributeApi ProductAttributeApi { get; set; }

        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public IToastService ToastService { get; set; }

        [Inject]
        public SweetAlertService SweetAlertService { get; set; }

        private List<BreadcrumbItem> breadcrumb = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Label = "Home", Url = "/", IsActive = false },
            new BreadcrumbItem { Label = "Product Templates", Url = "/product-templates", IsActive = true }
        };

        private List<ProductTemplateResponse> productTemplates = new();

        private List<ProductAttributeResponse> productAttributes = new();

        private ProductTemplateFormModel productTemplateForm = new ProductTemplateFormModel();

        private bool isLoading = true;

        private bool isEditMode = false;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await Task.WhenAll(GetProductTemplates(), GetProductAttributes());
        }

        private async Task GetProductAttributes()
        {
            var response = await ProductAttributeApi.GetProductAttributesAsync(1, 100);
            if (response.IsSuccess)
            {
                productAttributes = response.Data;
            }
        }

        private async Task GetProductTemplates()
        {
            var response = await ProductTemplateApi.GetProductTemplatesAsync(1, 100);
            if (response.IsSuccess)
            {
                productTemplates = response.Data;
            }
            isLoading = false;
        }

        private async Task ShowModal()
        {
            await JSRuntime.InvokeVoidAsync("externalLibs.showModal", "productTemplateModal");

            var groupedAttributes = productAttributes
                .GroupBy(x => x.GroupName)
                .Select(g => new
                {
                    groupName = g.Key,
                    attributes = g.Select(a => new { id = a.Id, name = a.Name }).ToList()
                })
                .ToList();

            var selectedValues = productTemplateForm.ProductAttributes?.Select(x => x.Id).ToArray() ?? Array.Empty<string>();

            await JSRuntime.InvokeVoidAsync("externalLibs.initSelect2WithGroups", 
                "#attributeSelect", 
                groupedAttributes,
                selectedValues,
                "Select attributes...", 
                "#productTemplateModal");
        }

        private async Task HideModal()
        {
            await JSRuntime.InvokeVoidAsync("externalLibs.hideModal", "productTemplateModal");
        }

        private async Task OpenAddModal()
        {
            isEditMode = false;
            productTemplateForm = new ProductTemplateFormModel();
            await ShowModal();
        }

        private async Task OpenEditModal(string productTemplateId)
        {
            isEditMode = true;
            var request = await ProductTemplateApi.GetProductTemplateAsync(productTemplateId);
            if (request.IsSuccess)
            {
                var productTemplateToEdit = request.Data;
                productTemplateForm = new ProductTemplateFormModel
                {
                    Id = productTemplateToEdit.Id,
                    Name = productTemplateToEdit.Name,
                    ProductAttributes = productTemplateToEdit.ProductAttributes?
                        .Select(x => new ProductAttributeItem { Id = x.Id, Name = x.Name })
                        .ToList() ?? new()
                };
                StateHasChanged();
                await Task.Delay(100);
                await ShowModal();
            }
            else
            {
                ToastService.ShowError(request.Message);
            }
        }

        private async Task OpenDeleteModal(string productTemplateId, string productTemplateName)
        {
            var confirmationResult = await SweetAlertService.FireAsync(new SweetAlertOptions
            {
                Title = "Confirm Product Template Deletion",
                Html = $"Are you sure you want to delete the product template <strong>{productTemplateName}</strong>? This action cannot be undone.",
                Icon = SweetAlertIcon.Warning,
                ShowCancelButton = true,
                ConfirmButtonText = "Yes, delete it!",
                CancelButtonText = "Cancel"
            });

            if (!confirmationResult.IsConfirmed)
            {
                return;
            }

            var deletedResult = await ProductTemplateApi.DeleteProductTemplateAsync(productTemplateId);

            if (deletedResult.IsSuccess)
            {
                await GetProductTemplates();
            }
            else
            {
                ToastService.ShowError(deletedResult.Message);
            }
        }

        private async Task FormSubmitted()
        {
            var selectedIds = await JSRuntime.InvokeAsync<string[]>("externalLibs.getSelect2Values", "#attributeSelect");
            
            productTemplateForm.ProductAttributes = selectedIds
                .Select(id => {
                    var attr = productAttributes.FirstOrDefault(a => a.Id == id);
                    return new ProductAttributeItem 
                    { 
                        Id = id, 
                        Name = attr?.Name ?? string.Empty 
                    };
                })
                .ToList();

            var response = isEditMode
                ? await ProductTemplateApi.UpdateProductTemplateAsync(productTemplateForm)
                : await ProductTemplateApi.CreateProductTemplateAsync(productTemplateForm);

            if (response.IsSuccess)
            {
                await GetProductTemplates();
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
