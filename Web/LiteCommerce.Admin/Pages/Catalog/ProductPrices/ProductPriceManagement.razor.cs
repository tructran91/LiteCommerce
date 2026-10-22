using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Models.Business.ProductPrice;
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

        private List<BreadcrumbItem> _breadcrumbs = new();

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
            _breadcrumbs = new()
            {
                new(L["Common_Home"], href: "/", icon: Icons.Material.Filled.Home),
                new(L["Menu_Catalog"], href: null, disabled: true),
                new(L["Price_Title"], href: null, disabled: true),
            };

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
                    Snackbar.Add(L["Price_NoChanges"], Severity.Warning);
                    return;
                }

                var response = await ProductPriceApi.UpdateProductPricingAsync(new UpdateProductPricingListRequest { Items = requests });
                if (response.IsSuccess)
                {
                    Snackbar.Add(L["Price_Updated", requests.Count], Severity.Success);
                    await LoadData();
                }
                else
                {
                    if (response.Errors != null && response.Errors.TryGetValue("notFoundProductIds", out var notFoundIds))
                    {
                        Snackbar.Add(L["Price_NotFound", string.Join(", ", notFoundIds)], Severity.Error);
                    }
                    else
                    {
                        Snackbar.Add(response.GetErrorMessage(L["Common_ErrorOccurred"]), Severity.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                Snackbar.Add(L["Price_SaveFailed", ex.Message], Severity.Error);
            }
            finally
            {
                _saving = false;
                StateHasChanged();
            }
        }
    }
}
