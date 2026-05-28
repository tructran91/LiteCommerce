using LiteCommerce.Admin.Models.Business.Product;
using Microsoft.AspNetCore.Components;

namespace LiteCommerce.Admin.Shared.Components
{
    public partial class ProductLinkSelector
    {
        [Parameter, EditorRequired]
        public string Title { get; set; } = string.Empty;

        [Parameter]
        public string Icon { get; set; } = "ti ti-link";

        [Parameter]
        public string EmptyMessage { get; set; } = "No products added.";

        [Parameter, EditorRequired]
        public IList<ProductLinkFormItem> SelectedProducts { get; set; } = [];

        [Parameter, EditorRequired]
        public List<BasicProductResponse> AllProducts { get; set; } = [];

        [Parameter]
        public string? ExcludeProductId { get; set; }

        [Parameter]
        public EventCallback<IList<ProductLinkFormItem>> SelectedProductsChanged { get; set; }

        private string? searchText;

        private List<BasicProductResponse> FilteredProducts { get; set; } = [];

        protected override void OnParametersSet()
        {
            RefreshFilteredProducts();
        }

        private void OnSearchTextChanged()
        {
            RefreshFilteredProducts();
        }

        private void RefreshFilteredProducts()
        {
            var results = AllProducts
                .Where(p => p.Id?.ToString() != ExcludeProductId)
                .Where(p => !SelectedProducts.Any(s => s.Id == p.Id?.ToString()));

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                results = results.Where(p => p.Name.Contains(searchText, StringComparison.OrdinalIgnoreCase));
            }

            FilteredProducts = results.ToList();
        }

        private async Task AddProduct(BasicProductResponse product)
        {
            var productId = product.Id?.ToString();
            if (SelectedProducts.Any(p => p.Id == productId))
                return;

            SelectedProducts.Add(new ProductLinkFormItem
            {
                Id = productId,
                Name = product.Name,
                IsPublished = product.IsPublished
            });

            RefreshFilteredProducts();
            await SelectedProductsChanged.InvokeAsync(SelectedProducts);
        }

        private async Task RemoveProduct(string productId)
        {
            var item = SelectedProducts.FirstOrDefault(p => p.Id == productId);
            if (item != null)
            {
                SelectedProducts.Remove(item);
                RefreshFilteredProducts();
                await SelectedProductsChanged.InvokeAsync(SelectedProducts);
            }
        }
    }
}
