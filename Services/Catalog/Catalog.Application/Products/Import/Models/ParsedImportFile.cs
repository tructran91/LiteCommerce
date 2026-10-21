namespace Catalog.Application.Products.Import.Models
{
    public class ParsedImportFile
    {
        public List<ParsedProductRow> Rows { get; } = new();

        // File-level problems (missing/unknown columns, empty workbook) that make every row unusable.
        public List<ProductImportError> Errors { get; } = new();

        public bool IsValid => Errors.Count == 0 && Rows.Count > 0;
    }
}
