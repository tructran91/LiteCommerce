namespace Catalog.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public string Slug { get; set; }

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public bool IsPublished { get; set; }

        // While true, the four fields below are derived: ProductService.RecalculateAggregates
        // rewrites them from the variants on every save, so editing them directly is lost.
        // Price and StockQuantity become the minimum and the sum across active variants.
        // Reservations are tracked per variant only; nothing reads a product-level total.
        public bool HasVariants { get; set; }

        public decimal Price { get; set; } = 0;

        // Upper bound of the range, so a list screen can render "from - to" without
        // joining the variants. Equal to Price when there are none.
        public decimal PriceMax { get; set; } = 0;

        public decimal? OldPrice { get; set; }

        public decimal? SpecialPrice { get; set; }

        public DateTime? SpecialPriceStart { get; set; }

        public DateTime? SpecialPriceEnd { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsCallForPricing { get; set; }

        public bool IsAllowToOrder { get; set; }

        public int StockQuantity { get; set; }

        public string? Sku { get; set; }

        public string? Gtin { get; set; }

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

        public Guid? BrandId { get; set; }

        public Brand? Brand { get; set; }

        public Media? ThumbnailImage { get; set; }

        public List<ProductMedia> Medias { get; set; } = new();

        public List<ProductLink> ProductLinks { get; set; } = new();

        public List<ProductLink> LinkedProductLinks { get; set; } = new();

        public List<ProductAttributeValue> AttributeValues { get; set; } = new();

        public List<ProductOptionValue> OptionValues { get; set; } = new();

        public List<ProductVariant> Variants { get; set; } = new();

        public List<ProductCategory> Categories { get; set; } = new();

        public List<ProductPriceHistory> PriceHistories { get; set; } = new();

        public void AddCategory(ProductCategory category)
        {
            category.Product = this;
            Categories.Add(category);
        }

        public void AddMedia(ProductMedia media)
        {
            media.Product = this;
            Medias.Add(media);
        }

        public void AddAttributeValue(ProductAttributeValue attributeValue)
        {
            attributeValue.Product = this;
            AttributeValues.Add(attributeValue);
        }

        public void AddOptionValue(ProductOptionValue optionValue)
        {
            optionValue.Product = this;
            OptionValues.Add(optionValue);
        }

        public void AddVariant(ProductVariant variant)
        {
            variant.Product = this;
            Variants.Add(variant);
        }

        public void AddProductLinks(ProductLink productLink)
        {
            productLink.Product = this;
            ProductLinks.Add(productLink);
        }
    }
}
