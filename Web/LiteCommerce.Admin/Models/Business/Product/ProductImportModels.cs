namespace LiteCommerce.Admin.Models.Business.Product
{
    public class ProductImportPreviewResponse
    {
        public int TotalRows { get; set; }

        public int ValidRows { get; set; }

        public int InvalidRows { get; set; }

        public List<ProductImportRowResult> Rows { get; set; } = new();

        public List<string> NewBrands { get; set; } = new();

        public List<ProductImportError> FileErrors { get; set; } = new();

        public bool CanCommit { get; set; }
    }

    public class ProductImportRowResult
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

        public List<ProductImportError> Errors { get; set; } = new();

        public List<string> Warnings { get; set; } = new();

        public bool IsValid => Status == ProductImportRowStatus.Valid;
    }

    public class ProductImportCategoryResult
    {
        public Guid Id { get; set; }

        public string Path { get; set; } = string.Empty;
    }

    public class ProductImportError
    {
        public string Column { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
    }

    public class ProductImportResultResponse
    {
        public int TotalRows { get; set; }

        public int Imported { get; set; }

        public int Skipped { get; set; }

        public List<string> CreatedBrands { get; set; } = new();

        public List<ProductImportRowFailure> Errors { get; set; } = new();
    }

    public class ProductImportRowFailure
    {
        public int RowNumber { get; set; }

        public string Name { get; set; } = string.Empty;

        public List<ProductImportError> Errors { get; set; } = new();
    }

    public static class ProductImportModes
    {
        public const string AllOrNothing = "AllOrNothing";
        public const string SkipInvalid = "SkipInvalid";
    }

    public static class ProductImportRowStatus
    {
        public const string Valid = "Valid";
        public const string Invalid = "Invalid";
    }

    public static class ProductImportBrandStatus
    {
        public const string Existing = "Existing";
        public const string New = "New";
        public const string NotFound = "NotFound";
        public const string None = "None";
    }

    public class ProductImportDialogResult
    {
        public int Imported { get; set; }

        public int Skipped { get; set; }
    }
}
