using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Models.Business.Brand;
using LiteCommerce.Admin.Pages.Base;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LiteCommerce.Admin.Pages.Catalog.Brands
{
    public partial class BrandManagement
    {
        [Inject]
        private IBrandApi BrandApi { get; set; }

        [Inject]
        private IDialogService DialogService { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        private DeleteOperationHelper _deleteHelper => new(DialogService, Snackbar, L);

        private List<BreadcrumbItem> _breadcrumbs = new();

        private MudTable<BrandResponse> _table = null!;
        private List<BrandResponse> _allBrands = new();
        private bool _loading = false;
        private string? _errorMessage;
        private BrandQuery _query = new();

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
                new(L["Brand_Title"], href: null, disabled: true),
            };

            await LoadData();
        }

        private async Task LoadData()
        {
            _loading = true;
            _errorMessage = null;

            try
            {
                var result = await BrandApi.GetBrandsAsync(_query.Page, _query.PageSize);
                if (result.IsSuccess)
                {
                    _allBrands = result.Data ?? new();
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

        private async Task OpenBrandDialog(bool isEdit, BrandResponse? brand = null)
        {
            var parameters = new DialogParameters
            {
                [nameof(BrandDialog.IsEdit)] = isEdit,
                [nameof(BrandDialog.InitialId)] = isEdit ? brand?.Id : null,
                [nameof(BrandDialog.InitialName)] = isEdit ? brand?.Name ?? "" : "",
                [nameof(BrandDialog.InitialIsPublished)] = isEdit ? brand?.IsPublished ?? true : true,
            };

            var dialog = await DialogService.ShowAsync<BrandDialog>("", parameters, _dialogOptions);
            var result = await dialog.Result;

            if (result is null || result.Canceled || result.Data is not BrandDialog.BrandDialogResult data)
                return;

            _loading = true;

            var brandForm = new BrandFormModel
            {
                Name = data.Name,
                IsPublished = data.IsPublished
            };

            try
            {
                var response = data.IsEdit
                    ? await BrandApi.UpdateBrandAsync(data.Id!, brandForm)
                    : await BrandApi.CreateBrandAsync(brandForm);

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

        private async Task OpenDeleteConfirm(BrandResponse brand)
        {
            _loading = true;

            try
            {
                await _deleteHelper.ExecuteDeleteOperation(
                    brand.Id,
                    brand.Name,
                    BrandApi.DeleteBrandAsync,
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
