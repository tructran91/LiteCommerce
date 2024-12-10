using Catalog.Core.Entities;

namespace Catalog.Core.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<Product> GetProductAsync(Guid id);
    }
}
