using Catalog.Core.Entities;

namespace Catalog.Application.Products.Import.Models
{
    // A ready-to-insert product. The import service resolves/creates the brand inside the transaction.
    public class ProductImportItem
    {
        public required Product Product { get; init; }

        public required string BrandName { get; init; }

        public Guid? BrandId { get; init; }

        public bool CreateBrandIfMissing { get; init; }

        public int RowNumber { get; init; }
    }

    public record ProductImportCommitResult(int ImportedCount, IReadOnlyList<string> CreatedBrands);
}
