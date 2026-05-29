using Catalog.Core.DTOs;
using Catalog.Core.Entities;

namespace Catalog.Core.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<Product> GetProductAsync(Guid id);

        Task<(List<ProductListItemDto> Products, int TotalCount)> GetProductsAsync(int currentPage, int pageSize);

        Task<(List<ProductPricingDto> Products, int TotalCount)> GetProductPricingAsync(int currentPage, int pageSize);
    }
}
