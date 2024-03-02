using Catalog.Application.ViewModels;
using Catalog.Core.Entities;
using Catalog.Core.Enums;

namespace Catalog.Application.Services
{
    public class ProductService : IProductService
    {
        public ProductPriceHistory CreatePriceHistory(Product product)
        {
            return new ProductPriceHistory
            {
                Product = product,
                Price = product.Price,
                OldPrice = product.OldPrice,
                SpecialPrice = product.SpecialPrice,
                SpecialPriceStart = product.SpecialPriceStart,
                SpecialPriceEnd = product.SpecialPriceEnd,
                CreatedOn = DateTime.UtcNow
            };
        }

        public void MapProductLinkToProduct(ProductViewModel productSource, Product productTarget)
        {
            foreach (var relatedProduct in productSource.RelatedProducts)
            {
                var productLink = new ProductLink
                {
                    LinkType = ProductLinkType.Related,
                    Product = productTarget,
                    LinkedProductId = Guid.Parse(relatedProduct.Id),
                    CreatedOn = DateTime.UtcNow
                };

                productTarget.AddProductLinks(productLink);
            }

            foreach (var crossSellProduct in productSource.CrossSellProducts)
            {
                var productLink = new ProductLink
                {
                    LinkType = ProductLinkType.CrossSell,
                    Product = productTarget,
                    LinkedProductId = Guid.Parse(crossSellProduct.Id),
                    CreatedOn = DateTime.UtcNow
                };

                productTarget.AddProductLinks(productLink);
            }
        }
    }
}
