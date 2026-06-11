using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Brand;
using LiteCommerce.Admin.Models.Business.ProductAttributeGroup;
using LiteCommerce.Admin.Models.Common;
using LiteCommerce.Admin.Pages.Base;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LiteCommerce.Admin.Pages.Catalog.ProductAttributeGroups
{
    public partial class ProductAttributeGroupManagement
    {
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
            new("Product Attribute Groups", href: null, disabled: true),
        };

        private MudTable<ProductAttributeGroupResponse> _table = null!;
        private List<ProductAttributeGroupResponse> _allGroups = new();
        private bool _loading = false;
        private string? _errorMessage;
        private ProductAttributeGroupQuery _query = new();

        private DialogOptions _dialogOptions = new()
        {
            MaxWidth = MaxWidth.Small,
            FullWidth = true,
            CloseOnEscapeKey = true,
            BackdropClick = false,
        };

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }

        private async Task LoadData()
        {
            _loading = true;
            _errorMessage = null;

            try
            {
                var result = await ProductAttributeGroupApi.GetProductAttributeGroupsAsync(_query.Page, _query.PageSize);
                if (result.IsSuccess)
                {
                    _allGroups = result.Data ?? new();
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

        private async Task OpenAttributeGroupDialog(bool isEdit, ProductAttributeGroupResponse? attributeGroup = null)
        {
            var parameters = new DialogParameters
            {
                [nameof(ProductAttributeGroupDialog.IsEdit)] = isEdit,
                [nameof(ProductAttributeGroupDialog.InitialId)] = isEdit ? attributeGroup?.Id : null,
                [nameof(ProductAttributeGroupDialog.InitialName)] = isEdit ? attributeGroup?.Name ?? "" : "",
            };

            var dialog = await DialogService.ShowAsync<ProductAttributeGroupDialog>("", parameters, _dialogOptions);
            var result = await dialog.Result;

            if (result is null || result.Canceled || result.Data is not ProductAttributeGroupDialog.ProductAttributeGroupDialogResult data)
                return;

            _loading = true;

            var attributeForm = new ProductAttributeGroupFormModel
            {
                Id = data.IsEdit ? data.Id : null,
                Name = data.Name
            };

            var response = data.IsEdit
                ? await ProductAttributeGroupApi.UpdateProductAttributeGroupAsync(attributeForm)
                : await ProductAttributeGroupApi.CreateProductAttributeGroupAsync(attributeForm);

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

        private async Task OpenDeleteConfirm(ProductAttributeGroupResponse attributeGroup)
        {
            _loading = true;

            await _deleteHelper.ExecuteDeleteOperation(
                attributeGroup.Id,
                attributeGroup.Name,
                ProductAttributeGroupApi.DeleteProductAttributeGroupAsync,
                async () => await LoadData()
            );

            _loading = false;
            StateHasChanged();
        }
    }
}
