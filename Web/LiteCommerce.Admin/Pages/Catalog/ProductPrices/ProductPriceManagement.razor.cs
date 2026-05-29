using Blazored.Toast.Services;
using LiteCommerce.Admin.ApiClients;
using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Application;
using LiteCommerce.Admin.Models.Business.ProductPrice;
using Microsoft.AspNetCore.Components;

namespace LiteCommerce.Admin.Pages.Catalog.ProductPrices
{
    public partial class ProductPriceManagement
    {
        [Inject]
        private IProductPriceApi ProductPriceApi { get; set; }

        [Inject]
        private IToastService ToastService { get; set; }

        private List<BreadcrumbItem> breadcrumb = new List<BreadcrumbItem>
        {
            new BreadcrumbItem { Label = "Home", Url = "/", IsActive = false },
            new BreadcrumbItem { Label = "Product Prices", Url = "/product-prices", IsActive = true }
        };

        private List<ProductPricingResponse> products = new();
        private bool isLoading = true;
        private bool isSaving = false;
        private int currentPage = 1;
        private int pageSize = AppConstants.PageSize;

        protected override async Task OnInitializedAsync()
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            isLoading = true;
            try
            {
                var response = await ProductPriceApi.GetProductPricingAsync(currentPage, pageSize);
                if (response.IsSuccess && response.Data != null)
                {
                    products = response.Data;

                    // Initialize original values for change tracking
                    foreach (var product in products)
                    {
                        product.OriginalPrice = product.Price;
                        product.OriginalOldPrice = product.OldPrice;
                    }
                }
                else
                {
                    ToastService.ShowError(response.Message);
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Error loading products: {ex.Message}");
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private async Task SavePricesAsync()
        {
            isSaving = true;
            try
            {
                var requests = products
                    .Where(p => p.HasAnyChange)
                    .Select(p => new UpdateProductPricingRequest
                    {
                        Id = p.Id,
                        NewPrice = p.HasPriceChanged ? p.Price : null,
                        NewOldPrice = p.HasOldPriceChanged ? p.OldPrice : null
                    })
                    .ToList();

                if (!requests.Any())
                {
                    ToastService.ShowWarning("No price changes to save");
                    return;
                }

                var response = await ProductPriceApi.UpdateProductPricingAsync(requests);
                if (response.IsSuccess)
                {
                    ToastService.ShowSuccess("Prices updated successfully");
                    await LoadDataAsync();
                }
                else
                {
                    if (response.Errors != null && response.Errors.ContainsKey("NotFoundProductIds"))
                    {
                        var notFoundIds = string.Join(", ", response.Errors["NotFoundProductIds"]);
                        ToastService.ShowError($"Products not found: {notFoundIds}");
                    }
                    else
                    {
                        ToastService.ShowError(response.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"Error saving prices: {ex.Message}");
            }
            finally
            {
                isSaving = false;
                StateHasChanged();
            }
        }
    }
}
