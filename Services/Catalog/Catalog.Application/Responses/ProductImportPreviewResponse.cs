namespace Catalog.Application.Responses
{
    public record ProductImportPreviewResponse
    {
        public int TotalRows { get; set; }

        public int ValidRows { get; set; }

        public int InvalidRows { get; set; }

        public List<ProductImportRowResult> Rows { get; set; } = new();

        public List<string> NewBrands { get; set; } = new();

        public List<ProductImportErrorResponse> FileErrors { get; set; } = new();

        public bool CanCommit { get; set; }
    }

    public record ProductImportRowResult
    {
        public int RowNumber { get; set; }

        public string Status { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Brand { get; set; } = string.Empty;

        public string BrandStatus { get; set; } = string.Empty;

        public Guid? BrandId { get; set; }

        public string? Category { get; set; }

        public List<ProductImportCategoryResult> Categories { get; set; } = new();

        public decimal? Price { get; set; }

        public List<ProductImportErrorResponse> Errors { get; set; } = new();

        public List<string> Warnings { get; set; } = new();
    }

    public record ProductImportCategoryResult
    {
        public Guid Id { get; set; }

        public string Path { get; set; } = string.Empty;
    }

    public record ProductImportErrorResponse
    {
        public string Column { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }
}
