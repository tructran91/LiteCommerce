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

        private DeleteOperationHelper _deleteHelper => new(DialogService, Snackbar);

        private List<BreadcrumbItem> _breadcrumbs = new()
        {
            new("Home", href: "/", icon: Icons.Material.Filled.Home),
            new("Catalog", href: null, disabled: true),
            new("Products", href: null, disabled: true),
        };

        private MudTable<BasicProductResponse> _table = null!;
        private readonly int[] _pageSizeOptions = { 10, AppConstants.PageSize, 50, 100 };
        private int _rowsPerPage = AppConstants.PageSize;
        private ProductQuery _query = new();
        private ProductQuery _appliedQuery = new();
        private bool _operationInProgress;
        private string? _errorMessage;

        private async Task<TableData<BasicProductResponse>> LoadServerData(TableState state, CancellationToken cancellationToken)
        {
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
                    return new TableData<BasicProductResponse>
                    {
                        Items = result.Data ?? new(),
                        TotalItems = result.Pagination?.TotalRecords ?? 0
                    };
                }

                _errorMessage = result.GetErrorMessage(SystemMessages.ErrorOccurred);
                Snackbar.Add(_errorMessage, Severity.Error);
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
                Snackbar.Add($"Error: {ex.Message}", Severity.Error);
            }

            return new TableData<BasicProductResponse> { Items = new List<BasicProductResponse>(), TotalItems = 0 };
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
                ? $"Imported {data.Imported} product(s), skipped {data.Skipped}."
                : $"Imported {data.Imported} product(s).";

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
                    Snackbar.Add(result.GetErrorMessage(SystemMessages.ErrorOccurred), Severity.Error);
                    return;
                }

                Snackbar.Add($"Copied product as \"{result.Data?.Name}\".", Severity.Success);
                await ReloadFromFirstPage();
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error: {ex.Message}", Severity.Error);
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
                    async () => await ReloadFromFirstPage()
                );
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error: {ex.Message}", Severity.Error);
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
