using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.ProductPrice;
using LiteCommerce.Admin.Models.Common;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace LiteCommerce.Admin.Pages.Catalog.ProductPrices
{
    public partial class ProductPriceManagement
    {
        [Inject]
        private IProductPriceApi ProductPriceApi { get; set; }

        [Inject]
        private ISnackbar Snackbar { get; set; }

        private List<BreadcrumbItem> _breadcrumbs = new()
        {
            new("Home", href: "/", icon: Icons.Material.Filled.Home),
            new("Catalog", href: null, disabled: true),
            new("Product Prices", href: null, disabled: true),
        };

        private MudTable<ProductPricingResponse> _table = null!;
        private BaseResponse<List<ProductPricingResponse>>? _pagedResult;
        private bool _loading = false;
        private bool _saving = false;
        private string? _errorMessage;
        private ProductPriceQuery _query = new();

        private bool HasAnyChanges => _pagedResult?.Data?.Any(p => p.HasAnyChange) ?? false;
        private int ChangedProductsCount => _pagedResult?.Data?.Count(p => p.HasAnyChange) ?? 0;

        private async Task<TableData<ProductPricingResponse>> ServerReload(TableState state, CancellationToken ct)
        {
            await LoadData();

            return new TableData<ProductPricingResponse>
            {
                Items = _pagedResult?.Data ?? new(),
                TotalItems = _pagedResult?.Pagination.TotalRecords ?? 0,
            };
        }

        private async Task LoadData()
        {
            _loading = true;
            _errorMessage = null;

            var result = await ProductPriceApi.GetProductPricingAsync(_query.Page, _query.PageSize);
            if (result.IsSuccess && result.Data != null)
            {
                _pagedResult = result;

                // Initialize original values for change tracking
                foreach (var product in _pagedResult.Data)
                {
                    product.OriginalPrice = product.Price;
                    product.OriginalOldPrice = product.OldPrice;
                }
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

        private void OnPriceChanged(ProductPricingResponse product, decimal newPrice)
        {
            product.Price = newPrice;
            StateHasChanged();
        }

        private void OnOldPriceChanged(ProductPricingResponse product, decimal? newOldPrice)
        {
            product.OldPrice = newOldPrice;
            StateHasChanged();
        }

        private async Task SavePricesAsync()
        {
            _saving = true;
            try
            {
                var requests = _pagedResult?.Data?
                    .Where(p => p.HasAnyChange)
                    .Select(p => new UpdateProductPricingRequest
                    {
                        Id = p.Id,
                        NewPrice = p.HasPriceChanged ? p.Price : null,
                        NewOldPrice = p.HasOldPriceChanged ? p.OldPrice : null
                    })
                    .ToList() ?? new();

                if (!requests.Any())
                {
                    Snackbar.Add("No price changes to save", Severity.Warning);
                    return;
                }

                var response = await ProductPriceApi.UpdateProductPricingAsync(requests);
                if (response.IsSuccess)
                {
                    Snackbar.Add($"Updated prices for {requests.Count} product(s) successfully", Severity.Success);
                    await ReloadTable();
                }
                else
                {
                    if (response.Errors != null && response.Errors.ContainsKey("NotFoundProductIds"))
                    {
                        var notFoundIds = string.Join(", ", response.Errors["NotFoundProductIds"]);
                        Snackbar.Add($"Products not found: {notFoundIds}", Severity.Error);
                    }
                    else
                    {
                        Snackbar.Add(response.Message ?? SystemMessages.ErrorOccurred, Severity.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add($"Error saving prices: {ex.Message}", Severity.Error);
            }
            finally
            {
                _saving = false;
                StateHasChanged();
            }
        }
    }
}
