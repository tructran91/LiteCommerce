using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.ProductOption;
using LiteCommerce.Admin.Models.Common;
using LiteCommerce.Admin.Pages.Base;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LiteCommerce.Admin.Pages.Catalog.ProductOptions
{
    public partial class ProductOptionManagement
    {
        [Inject]
        private IProductOptionApi ProductOptionApi { get; set; }

        [Inject]
        private IDialogService DialogService { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        private DeleteOperationHelper _deleteHelper => new(DialogService, Snackbar);

        private List<BreadcrumbItem> _breadcrumbs = new()
        {
            new("Home", href: "/", icon: Icons.Material.Filled.Home),
            new("Catalog", href: null, disabled: true),
            new("Product Options", href: null, disabled: true),
        };

        private MudTable<ProductOptionResponse> _table = null!;
        private List<ProductOptionResponse> _allOptions = new();
        private bool _loading = false;
        private string? _errorMessage;
        private ProductOptionQuery _query = new();

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
                var result = await ProductOptionApi.GetProductOptionsAsync(_query.Page, _query.PageSize);
                if (result.IsSuccess)
                {
                    _allOptions = result.Data ?? new();
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

        private async Task OpenOptionDialog(bool isEdit, ProductOptionResponse? option = null)
        {
            var parameters = new DialogParameters
            {
                [nameof(ProductOptionDialog.IsEdit)] = isEdit,
                [nameof(ProductOptionDialog.InitialId)] = isEdit ? option?.Id : null,
                [nameof(ProductOptionDialog.InitialName)] = isEdit ? option?.Name ?? "" : "",
            };

            var dialog = await DialogService.ShowAsync<ProductOptionDialog>("", parameters, _dialogOptions);
            var result = await dialog.Result;

            if (result is null || result.Canceled || result.Data is not ProductOptionDialog.ProductOptionDialogResult data)
                return;

            _loading = true;

            var optionForm = new ProductOptionFormModel
            {
                Id = data.IsEdit ? data.Id : null,
                Name = data.Name
            };

            var response = data.IsEdit
                ? await ProductOptionApi.UpdateProductOptionAsync(optionForm)
                : await ProductOptionApi.CreateProductOptionAsync(optionForm);

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

        private async Task OpenDeleteConfirm(ProductOptionResponse option)
        {
            _loading = true;

            await _deleteHelper.ExecuteDeleteOperation(
                option.Id,
                option.Name,
                ProductOptionApi.DeleteProductOptionAsync,
                async () => await LoadData()
            );

            _loading = false;
            StateHasChanged();
        }
    }
}
