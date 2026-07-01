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
        private List<ProductPricingResponse> _allPrices = new();
        private bool _loading = false;
        private bool _saving = false;
        private string? _errorMessage;
        private ProductPriceQuery _query = new();

        private bool HasAnyChanges => _allPrices?.Any(p => p.HasAnyChange) ?? false;
        private int ChangedProductsCount => _allPrices?.Count(p => p.HasAnyChange) ?? 0;

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
                var result = await ProductPriceApi.GetProductPricingAsync(_query.Page, _query.PageSize);
                if (result.IsSuccess && result.Data != null)
                {
                    _allPrices = result.Data;

                    // Initialize original values for change tracking
                    foreach (var product in _allPrices)
                    {
                        product.OriginalPrice = product.Price;
                        product.OriginalOldPrice = product.OldPrice;
                    }
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
                var requests = _allPrices?
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

                var response = await ProductPriceApi.UpdateProductPricingAsync(new UpdateProductPricingListRequest { Items = requests });
                if (response.IsSuccess)
                {
                    Snackbar.Add($"Updated prices for {requests.Count} product(s) successfully", Severity.Success);
                    await LoadData();
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
