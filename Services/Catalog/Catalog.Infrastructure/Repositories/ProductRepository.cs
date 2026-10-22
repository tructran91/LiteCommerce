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
            // Avoids cartesian explosion from the collection includes.
            var product = await _dbContext.Products
                .AsSplitQuery()
                .Include(t => t.ThumbnailImage)
                .Include(t => t.Medias).ThenInclude(t => t.Media)
                .Include(t => t.ProductLinks).ThenInclude(p => p.LinkedProduct).ThenInclude(m => m.ThumbnailImage)
                .Include(x => x.OptionValues).ThenInclude(o => o.Option)
                .Include(x => x.AttributeValues).ThenInclude(a => a.Attribute).ThenInclude(g => g.Group)
                .Include(x => x.Categories)
                .FirstOrDefaultAsync(x => x.Id == id);

            return product;
        }

        public async Task<(List<ProductListItemDto> Products, int TotalCount)> GetProductsAsync(
            ProductListFilter filter,
            int currentPage,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var query = _dbContext.Products.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                var name = filter.Name.Trim();
                query = query.Where(p => p.Name.Contains(name));
            }

            if (filter.IsFeatured.HasValue)
                query = query.Where(p => p.IsFeatured == filter.IsFeatured.Value);

            if (filter.IsAllowToOrder.HasValue)
                query = query.Where(p => p.IsAllowToOrder == filter.IsAllowToOrder.Value);

            if (filter.IsCallForPricing.HasValue)
                query = query.Where(p => p.IsCallForPricing == filter.IsCallForPricing.Value);

            if (filter.IsPublished.HasValue)
                query = query.Where(p => p.IsPublished == filter.IsPublished.Value);

            var totalCount = await query.CountAsync(cancellationToken);

            var descending = filter.SortDirection?.Equals("desc", StringComparison.OrdinalIgnoreCase) == true;
            var orderedQuery = filter.SortBy?.ToLowerInvariant() switch
            {
                "name" => descending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
                "isfeatured" => descending ? query.OrderByDescending(p => p.IsFeatured) : query.OrderBy(p => p.IsFeatured),
                "isallowtoorder" => descending ? query.OrderByDescending(p => p.IsAllowToOrder) : query.OrderBy(p => p.IsAllowToOrder),
                "iscallforpricing" => descending ? query.OrderByDescending(p => p.IsCallForPricing) : query.OrderBy(p => p.IsCallForPricing),
                "ispublished" => descending ? query.OrderByDescending(p => p.IsPublished) : query.OrderBy(p => p.IsPublished),
                _ => query.OrderByDescending(p => p.CreatedDate)
            };

            var products = await orderedQuery
                .ThenBy(p => p.Id)
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
                .ToListAsync(cancellationToken);

            return (products, totalCount);
        }

        public async Task<(List<ProductPricingDto> Products, int TotalCount)> GetProductPricingAsync(int currentPage, int pageSize)
        {
            var query = _dbContext.Products.AsNoTracking();

            var totalCount = await query.CountAsync();

            var products = await query
                .OrderBy(p => p.CreatedDate)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductPricingDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    OldPrice = p.OldPrice,
                    SpecialPrice = p.SpecialPrice,
                    SpecialPriceStart = p.SpecialPriceStart,
                    SpecialPriceEnd = p.SpecialPriceEnd
                })
                .ToListAsync();

            return (products, totalCount);
        }

        public async Task<List<ProductLink>> GetLinksInvolvingAsync(Guid productId)
        {
            return await _dbContext.ProductLinks
                .Where(l => l.ProductId == productId || l.LinkedProductId == productId)
                .ToListAsync();
        }

        public async Task<HashSet<string>> GetExistingSlugsAsync(IReadOnlyCollection<string> slugs, CancellationToken cancellationToken = default)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // SQL Server allows at most 2100 parameters per statement; batch the IN list.
            foreach (var chunk in slugs.Chunk(1000))
            {
                var values = chunk.ToList();
                var existing = await _dbContext.Products
                    .AsNoTracking()
                    .Where(p => values.Contains(p.Slug))
                    .Select(p => p.Slug)
                    .ToListAsync(cancellationToken);

                result.UnionWith(existing);
            }

            return result;
        }

        public async Task<HashSet<string>> GetExistingSkusAsync(IReadOnlyCollection<string> skus, CancellationToken cancellationToken = default)
        {
            var result = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var chunk in skus.Chunk(1000))
            {
                var values = chunk.ToList();
                var existing = await _dbContext.Products
                    .AsNoTracking()
                    .Where(p => p.Sku != null && values.Contains(p.Sku))
                    .Select(p => p.Sku!)
                    .ToListAsync(cancellationToken);

                result.UnionWith(existing);
            }

            return result;
        }
    }
}
