using Catalog.Core.Entities;

namespace Catalog.Core.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<List<Category>> GetCategoriesAsync();
    }
}
