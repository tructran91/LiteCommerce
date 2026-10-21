using Catalog.Core.DTOs;
using Catalog.Core.Entities;

namespace Catalog.Core.Repositories
{
    public interface IProductRepository : IBaseRepository<Product>
    {
        Task<Product> GetProductAsync(Guid id);

        Task<(List<ProductListItemDto> Products, int TotalCount)> GetProductsAsync(int currentPage, int pageSize);

        Task<(List<ProductPricingDto> Products, int TotalCount)> GetProductPricingAsync(int currentPage, int pageSize);

        // Tracked links where the product is either the source or the target.
        Task<List<ProductLink>> GetLinksInvolvingAsync(Guid productId);

        Task<HashSet<string>> GetExistingSlugsAsync(IReadOnlyCollection<string> slugs, CancellationToken cancellationToken = default);

        Task<HashSet<string>> GetExistingSkusAsync(IReadOnlyCollection<string> skus, CancellationToken cancellationToken = default);
    }
}
