namespace Catalog.Application.Products.Import
{
    // Column headers of the Excel template. Header row must use these exact names.
    public static class ProductImportTemplate
    {
        public const string ExcelContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        // Headers with this prefix are added by the error report and ignored on upload.
        public const string ReservedColumnPrefix = "_";

        public const string Name = "Name";
        public const string Brand = "Brand";
        public const string Category = "Category";
        public const string Price = "Price";
        public const string Slug = "Slug";
        public const string Sku = "Sku";
        public const string Gtin = "Gtin";
        public const string ShortDescription = "ShortDescription";
        public const string Description = "Description";
        public const string IsPublished = "IsPublished";
        public const string IsFeatured = "IsFeatured";
        public const string IsCallForPricing = "IsCallForPricing";
        public const string IsAllowToOrder = "IsAllowToOrder";
        public const string OldPrice = "OldPrice";
        public const string SpecialPrice = "SpecialPrice";
        public const string SpecialPriceStart = "SpecialPriceStart";
        public const string SpecialPriceEnd = "SpecialPriceEnd";
        public const string StockQuantity = "StockQuantity";
        public const string DisplayOrder = "DisplayOrder";
        public const string MetaTitle = "MetaTitle";
        public const string MetaKeywords = "MetaKeywords";
        public const string MetaDescription = "MetaDescription";
        public const string CanonicalUrl = "CanonicalUrl";
        public const string OgTitle = "OgTitle";
        public const string OgDescription = "OgDescription";
        public const string OgImage = "OgImage";
        public const string OgUrl = "OgUrl";
        public const string SchemaJson = "SchemaJson";

        public static readonly string[] RequiredColumns = [Name, Price];

        public static readonly HashSet<string> AllColumns = new(StringComparer.OrdinalIgnoreCase)
        {
            Name, Brand, Category, Price, Slug, Sku, Gtin, ShortDescription, Description,
            IsPublished, IsFeatured, IsCallForPricing, IsAllowToOrder,
            OldPrice, SpecialPrice, SpecialPriceStart, SpecialPriceEnd,
            StockQuantity, DisplayOrder,
            MetaTitle, MetaKeywords, MetaDescription, CanonicalUrl,
            OgTitle, OgDescription, OgImage, OgUrl, SchemaJson
        };
    }
}
