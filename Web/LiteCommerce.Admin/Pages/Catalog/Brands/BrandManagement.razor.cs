using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Brand;
using LiteCommerce.Admin.Models.Common;
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

        private DeleteOperationHelper _deleteHelper => new(DialogService, Snackbar);

        private List<BreadcrumbItem> _breadcrumbs = new()
        {
            new("Home", href: "/", icon: Icons.Material.Filled.Home),
            new("Catalog", href: null, disabled: true),
            new("Brands", href: null, disabled: true),
        };

        private MudTable<BrandResponse> _table = null!;
        private BaseResponse<List<BrandResponse>>? _pagedResult;
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

        private async Task<TableData<BrandResponse>> ServerReload(TableState state, CancellationToken ct)
        {
            await LoadData();

            return new TableData<BrandResponse>
            {
                Items = _pagedResult?.Data ?? new(),
                TotalItems = _pagedResult?.Pagination.TotalRecords ?? 0,
            };
        }

        private async Task LoadData()
        {
            _loading = true;
            _errorMessage = null;

            var result = await BrandApi.GetBrandsAsync(_query.Page, _query.PageSize);
            if (result.IsSuccess)
            {
                _pagedResult = result;
            }
            else
            {
                _errorMessage = result.Message;
                Snackbar.Add(result.Message ?? SystemMessages.ErrorOccurred, Severity.Error);
            }

            _loading = false;
            StateHasChanged();
        }

        private async Task ReloadTable() => await _table.ReloadServerData();

        private async Task OnPageChanged(int page)
        {
            _query.Page = page;
            await ReloadTable();
        }

        private async Task OnPageSizeChanged(int size)
        {
            _query.PageSize = size;
            _query.Page = 1;
            await ReloadTable();
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
                Id = data.IsEdit ? data.Id : null,
                Name = data.Name,
                IsPublished = data.IsPublished
            };

            var response = data.IsEdit
                ? await BrandApi.UpdateBrandAsync(brandForm)
                : await BrandApi.CreateBrandAsync(brandForm);

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

            await ReloadTable();
            _loading = false;
            StateHasChanged();
        }

        private async Task OpenDeleteConfirm(BrandResponse brand)
        {
            _loading = true;

            await _deleteHelper.ExecuteDeleteOperation(
                brand.Id,
                brand.Name,
                BrandApi.DeleteBrandAsync,
                ReloadTable
            );

            _loading = false;
            StateHasChanged();
        }
    }
}
