namespace Catalog.Application.Products.Import.Models
{
    public record ProductImportAnalysis(
        ParsedImportFile File,
        IReadOnlyList<ResolvedProductRow> Rows,
        IReadOnlyList<string> NewBrands);
}
