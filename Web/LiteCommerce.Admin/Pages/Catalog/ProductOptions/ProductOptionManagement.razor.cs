using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Models.Business.ProductOption;
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

        [Inject]
        private DeleteOperationHelper DeleteHelper { get; set; }

        private List<BreadcrumbItem> _breadcrumbs = new();

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
            _breadcrumbs = new()
            {
                new(L["Common_Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(L["Menu_Catalog"], href: null, disabled: true),
                new(L["Option_Title"], href: null, disabled: true),
            };

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
                    var errorDetails = result.GetErrorMessage(L["Common_ErrorOccurred"]);
                    _errorMessage = errorDetails;
                    Snackbar.Add(errorDetails, Severity.Error);
                }
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                Snackbar.Add(L["Common_ErrorWithDetail", ex.Message], Severity.Error);
            }
            finally
            {
                _loading = false;
                StateHasChanged();
            }
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
                Name = data.Name
            };

            try
            {
                var response = data.IsEdit
                    ? await ProductOptionApi.UpdateProductOptionAsync(data.Id!, optionForm)
                    : await ProductOptionApi.CreateProductOptionAsync(optionForm);

                if (response.IsSuccess)
                {
                    var successMessage = data.IsEdit
                        ? L["Common_UpdateDataSuccess"]
                        : L["Common_AddDataSuccess"];

                    Snackbar.Add(successMessage, Severity.Success);
                    await LoadData();
                }
                else
                {
                    Snackbar.Add(response.GetErrorMessage(L["Common_ErrorOccurred"]), Severity.Error);
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add(L["Common_ErrorWithDetail", ex.Message], Severity.Error);
            }
            finally
            {
                _loading = false;
                StateHasChanged();
            }
        }

        private async Task OpenDeleteConfirm(ProductOptionResponse option)
        {
            _loading = true;

            try
            {
                await DeleteHelper.ExecuteDeleteOperation(
                    option.Id,
                    option.Name,
                    ProductOptionApi.DeleteProductOptionAsync,
                    async () => await LoadData()
                );
            }
            catch (Exception ex)
            {
                Snackbar.Add(L["Common_ErrorWithDetail", ex.Message], Severity.Error);
            }
            finally
            {
                _loading = false;
                StateHasChanged();
            }
        }
    }
}
