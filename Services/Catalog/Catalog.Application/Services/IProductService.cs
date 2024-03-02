using Catalog.Application.ViewModels;
using Catalog.Core.Entities;

namespace Catalog.Application.Services
{
    public interface IProductService
    {
        ProductPriceHistory CreatePriceHistory(Product product);

        void MapProductLinkToProduct(ProductViewModel productSource, Product productTarget);
    }
}
