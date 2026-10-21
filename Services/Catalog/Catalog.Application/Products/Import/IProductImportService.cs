using Catalog.Application.Products.Import.Models;
using Catalog.Core.Entities;

namespace Catalog.Application.Products.Import
{
    // Implemented in Infrastructure: owns the transaction and the CatalogContext.
    public interface IProductImportService
    {
        Task<ProductImportCommitResult> ImportAsync(IReadOnlyList<ProductImportItem> items, CancellationToken cancellationToken = default);
    }
}
