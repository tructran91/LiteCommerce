namespace Catalog.Application.Products.Import.Models
{
    public class ResolvedProductRow
    {
        public required ParsedProductRow Source { get; init; }

        public int RowNumber => Source.RowNumber;

        public string Name => Source.Name;

        public string BrandName => Source.Brand;

        public string Slug { get; set; } = string.Empty;

        public BrandResolutionStatus BrandStatus { get; set; }

        public Guid? BrandId { get; set; }

        public List<ResolvedCategory> Categories { get; } = new();

        public List<ProductImportError> Errors { get; } = new();

        public List<string> Warnings { get; } = new();

        public bool IsValid => Errors.Count == 0;
    }

    public enum BrandResolutionStatus
    {
        Existing = 1,
        New,
        NotFound,
        // The row left the Brand cell empty. The product is imported without a brand.
        None
    }

    public record ResolvedCategory(Guid Id, string Path);
}
