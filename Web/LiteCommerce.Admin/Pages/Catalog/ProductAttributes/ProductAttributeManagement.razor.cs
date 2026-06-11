using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.ProductAttribute;
using LiteCommerce.Admin.Models.Business.ProductAttributeGroup;
using LiteCommerce.Admin.Models.Common;
using LiteCommerce.Admin.Pages.Base;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LiteCommerce.Admin.Pages.Catalog.ProductAttributes
{
    public partial class ProductAttributeManagement
    {
        [Inject]
        private IProductAttributeApi ProductAttributeApi { get; set; }

        [Inject]
        private IProductAttributeGroupApi ProductAttributeGroupApi { get; set; }

        [Inject]
        private IDialogService DialogService { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        private DeleteOperationHelper _deleteHelper => new(DialogService, Snackbar);

        private List<BreadcrumbItem> _breadcrumbs = new()
        {
            new("Home", href: "/", icon: Icons.Material.Filled.Home),
            new("Catalog", href: null, disabled: true),
            new("Product Attributes", href: null, disabled: true),
        };

        private MudTable<ProductAttributeResponse> _table = null!;
        private List<ProductAttributeResponse> _allAttributes = new();
        private List<ProductAttributeGroupResponse> _productAttributeGroups = new();
        private bool _loading = false;
        private string? _errorMessage;
        private ProductAttributeQuery _query = new();

        private DialogOptions _dialogOptions = new()
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            CloseOnEscapeKey = true,
            BackdropClick = false,
        };

        protected override async Task OnInitializedAsync()
        {
            await LoadProductAttributeGroups();
            await LoadData();
        }

        private async Task LoadData()
        {
            _loading = true;
            _errorMessage = null;

            try
            {
                var result = await ProductAttributeApi.GetProductAttributesAsync(_query.Page, _query.PageSize);
                if (result.IsSuccess)
                {
                    _allAttributes = result.Data ?? new();
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

        private async Task LoadProductAttributeGroups()
        {
            var response = await ProductAttributeGroupApi.GetProductAttributeGroupsAsync(1, 100);
            if (response.IsSuccess)
            {
                _productAttributeGroups = response.Data;
            }
        }

        private async Task OpenAttributeDialog(bool isEdit, ProductAttributeResponse? productAttribute = null)
        {
            var parameters = new DialogParameters
            {
                [nameof(ProductAttributeDialog.IsEdit)] = isEdit,
                [nameof(ProductAttributeDialog.InitialId)] = isEdit ? productAttribute?.Id : null,
                [nameof(ProductAttributeDialog.InitialName)] = isEdit ? productAttribute?.Name ?? "" : "",
                [nameof(ProductAttributeDialog.InitialGroupId)] = isEdit ? productAttribute?.GroupId ?? "" : "",
                [nameof(ProductAttributeDialog.Groups)] = _productAttributeGroups,
            };

            var dialog = await DialogService.ShowAsync<ProductAttributeDialog>("", parameters, _dialogOptions);
            var result = await dialog.Result;

            if (result is null || result.Canceled || result.Data is not ProductAttributeDialog.ProductAttributeDialogResult data)
                return;

            _loading = true;

            var attributeForm = new ProductAttributeFormModel
            {
                Id = data.IsEdit ? data.Id : null,
                Name = data.Name,
                GroupId = data.GroupId
            };

            var response = data.IsEdit
                ? await ProductAttributeApi.UpdateProductAttributeAsync(attributeForm)
                : await ProductAttributeApi.CreateProductAttributeAsync(attributeForm);

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

        private async Task OpenDeleteConfirm(ProductAttributeResponse attribute)
        {
            _loading = true;

            await _deleteHelper.ExecuteDeleteOperation(
                attribute.Id,
                attribute.Name,
                ProductAttributeApi.DeleteProductAttributeAsync,
                async () => await LoadData()
            );

            _loading = false;
            StateHasChanged();
        }
    }
}
