using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.ProductAttribute;
using LiteCommerce.Admin.Models.Business.ProductTemplate;
using LiteCommerce.Admin.Models.Common;
using LiteCommerce.Admin.Pages.Base;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LiteCommerce.Admin.Pages.Catalog.ProductTemplates
{
    public partial class ProductTemplateManagement
    {
        [Inject]
        private IProductTemplateApi ProductTemplateApi { get; set; }

        [Inject]
        private IProductAttributeApi ProductAttributeApi { get; set; }

        [Inject]
        private IDialogService DialogService { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        private DeleteOperationHelper _deleteHelper => new(DialogService, Snackbar);

        private List<BreadcrumbItem> _breadcrumbs = new()
        {
            new("Home", href: "/", icon: Icons.Material.Filled.Home),
            new("Catalog", href: null, disabled: true),
            new("Product Templates", href: null, disabled: true),
        };

        private MudTable<ProductTemplateResponse> _table = null!;
        private List<ProductTemplateResponse> _allTemplates = new();
        private List<ProductAttributeResponse> _productAttributes = new();
        private bool _loading = false;
        private string? _errorMessage;
        private ProductTemplateQuery _query = new();

        private DialogOptions _dialogOptions = new()
        {
            MaxWidth = MaxWidth.Medium,
            FullWidth = true,
            CloseOnEscapeKey = true,
            BackdropClick = false,
        };

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            await LoadProductAttributes();
            await LoadData();
        }

        private async Task LoadProductAttributes()
        {
            var response = await ProductAttributeApi.GetProductAttributesAsync(1, 100);
            if (response.IsSuccess)
            {
                _productAttributes = response.Data;
            }
        }

        private async Task LoadData()
        {
            _loading = true;
            _errorMessage = null;

            try
            {
                var result = await ProductTemplateApi.GetProductTemplatesAsync(_query.Page, _query.PageSize);
                if (result.IsSuccess)
                {
                    _allTemplates = result.Data ?? new();
                }
                else
                {
                    var errorDetails = result.Errors != null && result.Errors.Any()
                        ? string.Join(", ", result.Errors.SelectMany(e => e.Value.Select(msg => $"{e.Key}: {msg}")))
                        : result.Message ?? SystemMessages.ErrorOccurred;

                    _errorMessage = errorDetails;
                    Snackbar.Add(errorDetails, Severity.Error);
                }
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                Snackbar.Add($"Error: {ex.Message}", Severity.Error);
            }

            _loading = false;
            StateHasChanged();
        }

        private void OnSortChanged(SortDirection direction)
        {
            _table.NavigateTo(Page.First);
        }

        private async Task OpenTemplateDialog(bool isEdit, ProductTemplateResponse? productTemplate = null)
        {
            var parameters = new DialogParameters
            {
                [nameof(ProductTemplateDialog.IsEdit)] = isEdit,
                [nameof(ProductTemplateDialog.InitialId)] = isEdit ? productTemplate?.Id : null,
                [nameof(ProductTemplateDialog.InitialName)] = isEdit ? productTemplate?.Name ?? "" : "",
                [nameof(ProductTemplateDialog.AllAttributes)] = _productAttributes,
                [nameof(ProductTemplateDialog.InitialSelectedAttributes)] = isEdit ? productTemplate?.ProductAttributes : null,
            };

            var dialog = await DialogService.ShowAsync<ProductTemplateDialog>("", parameters, _dialogOptions);
            var result = await dialog.Result;

            if (result is null || result.Canceled || result.Data is not ProductTemplateDialog.TemplateDialogResult data)
                return;

            _loading = true;

            var templateForm = new ProductTemplateFormModel
            {
                Id = data.IsEdit ? data.Id : null,
                Name = data.Name,
                ProductAttributes = data.ProductAttributes
            };

            var response = data.IsEdit
                ? await ProductTemplateApi.UpdateProductTemplateAsync(templateForm)
                : await ProductTemplateApi.CreateProductTemplateAsync(templateForm);

            if (response.IsSuccess)
            {
                var successMessage = data.IsEdit
                    ? SystemMessages.UpdateDataSuccess
                    : SystemMessages.AddDataSuccess;

                Snackbar.Add(successMessage, Severity.Success);
            }
            else
            {
                var msg = response.Message ?? SystemMessages.ErrorOccurred;
                Snackbar.Add(msg, Severity.Error);
            }

            await LoadData();
            _loading = false;
            StateHasChanged();
        }

        private async Task OpenDeleteConfirm(ProductTemplateResponse productTemplate)
        {
            _loading = true;

            await _deleteHelper.ExecuteDeleteOperation(
                productTemplate.Id,
                productTemplate.Name,
                ProductTemplateApi.DeleteProductTemplateAsync,
                async () => await LoadData()
            );

            _loading = false;
            StateHasChanged();
        }
    }
}
