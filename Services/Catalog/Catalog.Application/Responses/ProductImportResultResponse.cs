namespace Catalog.Application.Responses
{
    public record ProductImportResultResponse
    {
        public int TotalRows { get; set; }

        public int Imported { get; set; }

        public int Skipped { get; set; }

        public List<string> CreatedBrands { get; set; } = new();

        public List<ProductImportRowFailure> Errors { get; set; } = new();
    }

    public record ProductImportRowFailure
    {
        public int RowNumber { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<ProductImportErrorResponse> Errors { get; set; } = new();
    }
}
