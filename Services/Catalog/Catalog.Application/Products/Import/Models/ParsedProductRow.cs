namespace Catalog.Application.Products.Import.Models
{
    // Raw values read from one worksheet row. Parse errors are collected instead of thrown
    // so a single bad cell does not stop the whole file.
    public class ParsedProductRow
    {
        public int RowNumber { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Brand { get; set; } = string.Empty;

        public string? Category { get; set; }

        public decimal? Price { get; set; }

        public string? Slug { get; set; }

        public string? Sku { get; set; }

        public string? Gtin { get; set; }

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public bool IsPublished { get; set; } = true;

        public bool IsFeatured { get; set; }

        public bool IsCallForPricing { get; set; }

        public bool IsAllowToOrder { get; set; }

        public decimal? OldPrice { get; set; }

        public decimal? SpecialPrice { get; set; }

        public DateTime? SpecialPriceStart { get; set; }

        public DateTime? SpecialPriceEnd { get; set; }

        public int StockQuantity { get; set; }

        public int DisplayOrder { get; set; }

        public string? MetaTitle { get; set; }

        public string? MetaKeywords { get; set; }

        public string? MetaDescription { get; set; }

        public string? CanonicalUrl { get; set; }

        public string? OgTitle { get; set; }

        public string? OgDescription { get; set; }

        public string? OgImage { get; set; }

        public string? OgUrl { get; set; }

        public string? SchemaJson { get; set; }

        public List<ProductImportError> Errors { get; } = new();
    }
}
