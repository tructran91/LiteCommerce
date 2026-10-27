using Catalog.Application.Responses;
using Catalog.Application.ViewModels;
using Catalog.Core.Entities;

namespace Catalog.Application.Services
{
    public interface IProductService
    {
        ProductPriceHistory CreatePriceHistory(Product product);

        ProductResponse BuildProductResponse(Product product);

        void AddOrDeleteProductLinks(ProductViewModel productSource, Product productTarget);

        void AddOrDeleteOptions(ProductViewModel productSource, Product productTarget);

        // After AddOrDeleteOptions: variants resolve against the items that call creates.
        void AddOrDeleteVariants(ProductViewModel productSource, Product productTarget);

        // After AddOrDeleteVariants. Rewrites HasVariants, Price, PriceMax, OldPrice
        // and StockQuantity on the product.
        void RecalculateAggregates(Product product);

        void AddOrDeleteAttributes(ProductViewModel productSource, Product productTarget);

        void AddOrDeleteCategories(ProductViewModel productSource, Product productTarget);
    }
}
