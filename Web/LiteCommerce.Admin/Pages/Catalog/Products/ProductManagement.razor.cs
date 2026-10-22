using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.Product;
using LiteCommerce.Admin.Pages.Base;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;

namespace LiteCommerce.Admin.Pages.Catalog.Products
{
    public partial class ProductManagement
    {
        [Inject]
        private IProductApi ProductApi { get; set; }

        [Inject]
        private IDialogService DialogService { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private DeleteOperationHelper _deleteHelper => new(DialogService, Snackbar, L);

        private List<BreadcrumbItem> _breadcrumbs = new();

        private MudTable<BasicProductResponse> _table = null!;
        private readonly int[] _pageSizeOptions = { 10, AppConstants.PageSize, 50, 100 };
        private int _rowsPerPage = AppConstants.PageSize;
        private ProductQuery _query = new();
        private ProductQuery _appliedQuery = new();
        private bool _operationInProgress;
        private string? _errorMessage;

        protected override void OnInitialized()
        {
            _breadcrumbs = new()
            {
                new(L["Common_Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(L["Menu_Catalog"], href: null, disabled: true),
                new(L["Product_Title"], href: null, disabled: true),
            };
        }

        private async Task<TableData<BasicProductResponse>> LoadServerData(TableState state, CancellationToken cancellationToken)
        {
            var hadError = _errorMessage != null;
            _errorMessage = null;

            try
            {
                var name = string.IsNullOrWhiteSpace(_appliedQuery.Name) ? null : _appliedQuery.Name.Trim();
                var sortDirection = state.SortDirection switch
                {
                    SortDirection.Ascending => "asc",
                    SortDirection.Descending => "desc",
                    _ => null
                };

                var result = await ProductApi.GetProductsAsync(
                    state.Page + 1,
                    state.PageSize,
                    name,
                    _appliedQuery.IsFeatured,
                    _appliedQuery.IsAllowToOrder,
                    _appliedQuery.IsCallForPricing,
                    _appliedQuery.IsPublished,
                    sortDirection is null ? null : state.SortLabel,
                    sortDirection);

                if (result.IsSuccess)
                {
                    if (hadError)
                        StateHasChanged();

                    return new TableData<BasicProductResponse>
                    {
                        Items = result.Data ?? new(),
                        TotalItems = result.Pagination?.TotalRecords ?? 0
                    };
                }

                _errorMessage = result.GetErrorMessage(L["Common_ErrorOccurred"]);
                Snackbar.Add(_errorMessage, Severity.Error);
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                Snackbar.Add(L["Common_ErrorWithDetail", ex.Message], Severity.Error);
            }

            // The table re-renders only itself; the page must re-render to show the error alert.
            StateHasChanged();
            return new TableData<BasicProductResponse> { Items = new List<BasicProductResponse>(), TotalItems = 0 };
        }

        private async Task ReloadCurrentPage()
        {
            await _table.ReloadServerData();
        }

        private async Task ReloadAfterDelete()
        {
            if (_table.CurrentPage > 0 && _table.FilteredItems.Count() <= 1)
                _table.NavigateTo(Page.Previous);
            else
                await _table.ReloadServerData();
        }

        private async Task ApplyFilter()
        {
            _appliedQuery = CopyQuery(_query);
            await ReloadFromFirstPage();
        }

        private async Task ReloadFromFirstPage()
        {
            if (_table.CurrentPage == 0)
                await _table.ReloadServerData();
            else
                _table.NavigateTo(Page.First);
        }

        private async Task ClearFilter()
        {
            _query = new();
            _appliedQuery = new();
            await ApplyFilter();
        }

        private async Task OnSearchKeyUp(KeyboardEventArgs e)
        {
            if (e.Key == "Enter")
                await ApplyFilter();
        }

        private void NavigateToAdd()
        {
            NavigationManager.NavigateTo("/products/add");
        }

        private async Task OpenImportDialog()
        {
            var options = new DialogOptions
            {
                MaxWidth = MaxWidth.Large,
                FullWidth = true,
                // Escape would bypass the dialog's own close logic (busy guard, reload after import).
                CloseOnEscapeKey = false,
                BackdropClick = false,
            };

            var dialog = await DialogService.ShowAsync<ProductImportDialog>("", new DialogParameters(), options);
            var result = await dialog.Result;

            if (result is null || result.Canceled || result.Data is not ProductImportDialogResult data)
                return;

            var message = data.Skipped > 0
                ? L["Product_ImportToast", data.Imported, data.Skipped]
                : L["Product_ImportToastNoSkip", data.Imported];

            Snackbar.Add(message, Severity.Success);
            await ReloadFromFirstPage();
        }

        private void NavigateToEdit(string id)
        {
            NavigationManager.NavigateTo($"/products/edit/{id}");
        }

        private async Task CopyProduct(BasicProductResponse product)
        {
            if (!product.Id.HasValue)
                return;

            _operationInProgress = true;

            try
            {
                var result = await ProductApi.CopyProductAsync(product.Id.Value.ToString());
                if (!result.IsSuccess)
                {
                    Snackbar.Add(result.GetErrorMessage(L["Common_ErrorOccurred"]), Severity.Error);
                    return;
                }

                Snackbar.Add(L["Product_CopiedToast", result.Data?.Name], Severity.Success);
                await ReloadFromFirstPage();
            }
            catch (Exception ex)
            {
                Snackbar.Add(L["Common_ErrorWithDetail", ex.Message], Severity.Error);
            }
            finally
            {
                _operationInProgress = false;
                StateHasChanged();
            }
        }

        private async Task OpenDeleteConfirm(BasicProductResponse product)
        {
            _operationInProgress = true;

            try
            {
                await _deleteHelper.ExecuteDeleteOperation(
                    product.Id.ToString(),
                    product.Name,
                    ProductApi.DeleteProductAsync,
                    ReloadAfterDelete
                );
            }
            catch (Exception ex)
            {
                Snackbar.Add(L["Common_ErrorWithDetail", ex.Message], Severity.Error);
            }
            finally
            {
                _operationInProgress = false;
                StateHasChanged();
            }
        }

        private static ProductQuery CopyQuery(ProductQuery source)
        {
            return new ProductQuery
            {
                Name = source.Name,
                IsFeatured = source.IsFeatured,
                IsAllowToOrder = source.IsAllowToOrder,
                IsCallForPricing = source.IsCallForPricing,
                IsPublished = source.IsPublished
            };
        }
    }
}
