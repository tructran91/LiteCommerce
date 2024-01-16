using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(CatalogContext dbContext) : base(dbContext)
        {
        }

        //public async Task<List<Product>> GetProductsAsync(ProductCriteriaDto criteria)
        //{
        //    IQueryable<Product> query = _dbContext.Products.AsNoTracking();

        //    if (!string.IsNullOrEmpty(criteria.SearchKeyword))
        //    {
        //        query = query.Where(t => t.Name.ToLower().Contains(criteria.SearchKeyword, StringComparison.OrdinalIgnoreCase));
        //    }

        //    // làm sao để đưa cái biến này ra ngoài tầng service 1 cách đẹp trai nhất
        //    int totalRecord = await query.CountAsync();

        //    if (!string.IsNullOrEmpty(criteria.SortColumn))
        //    {
        //        query = query.OrderBy(t => t.Name);
        //    }

        //    var result = await query.Skip((criteria.PageNumber - 1) * criteria.PageSize)
        //        .Take(criteria.PageSize)
        //        .ToListAsync();

        //    // phân trang nên để ở tầng repo hay tầng service

        //    return null;
        //}
    }
}
