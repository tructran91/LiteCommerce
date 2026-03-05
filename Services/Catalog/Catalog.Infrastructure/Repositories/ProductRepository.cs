using Catalog.Core.DTOs;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories
{
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(CatalogContext dbContext) : base(dbContext)
        {
        }

        public async Task<Product> GetProductAsync(Guid id)
        {
            var product = _dbContext.Products
                .Include(t => t.ThumbnailImage)
                .Include(t => t.Medias).ThenInclude(t => t.Media)
                .Include(t => t.ProductLinks).ThenInclude(p => p.LinkedProduct).ThenInclude(m => m.ThumbnailImage)
                .Include(x => x.OptionValues).ThenInclude(o => o.Option)
                .Include(x => x.AttributeValues).ThenInclude(a => a.Attribute).ThenInclude(g => g.Group)
                .Include(x => x.Categories)
                .FirstOrDefault(x => x.Id == id);

            return product;
        }

        public async Task<(List<ProductListItemDto> Products, int TotalCount)> GetProductsAsync(int currentPage, int pageSize)
        {
            var query = _dbContext.Products.AsNoTracking().Where(p => !p.IsDeleted);

            var totalCount = await query.CountAsync();

            var products = await query
                .OrderBy(p => p.CreatedDate)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductListItemDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    IsFeatured = p.IsFeatured,
                    IsAllowToOrder = p.IsAllowToOrder,
                    IsCallForPricing = p.IsCallForPricing,
                    IsPublished = p.IsPublished
                })
                .ToListAsync();

            return (products, totalCount);
        }
    }
}
