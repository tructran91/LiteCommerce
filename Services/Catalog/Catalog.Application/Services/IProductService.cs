using Catalog.Application.ViewModels;
using Catalog.Core.Entities;

namespace Catalog.Application.Services
{
    public interface IProductService
    {
        ProductPriceHistory CreatePriceHistory(Product product);

        void AddOrDeleteProductLinks(ProductViewModel productSource, Product productTarget);

        void AddOrDeleteOptions(ProductViewModel productSource, Product productTarget);

        void AddOrDeleteAttributes(ProductViewModel productSource, Product productTarget);

        void AddOrDeleteCategories(ProductViewModel productSource, Product productTarget);
    }
}
