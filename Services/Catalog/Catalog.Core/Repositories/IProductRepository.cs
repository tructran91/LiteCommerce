using Catalog.Core.Entities;

namespace Catalog.Core.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<Product> GetProductAsync(Guid id);
    }
}
