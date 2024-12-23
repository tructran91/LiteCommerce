namespace Catalog.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public string Slug { get; set; }

        public string? ShortDescription { get; set; }

        public string? Description { get; set; }

        public bool IsPublished { get; set; }

        public decimal Price { get; set; } = 0;

        public decimal? OldPrice { get; set; }

        public decimal? SpecialPrice { get; set; }

        public DateTime? SpecialPriceStart { get; set; }

        public DateTime? SpecialPriceEnd { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsCallForPricing { get; set; }

        public bool IsAllowToOrder { get; set; }

        public int StockQuantity { get; set; }

        public string? Sku { get; set; }

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

        public Guid BrandId { get; set; }

        public Brand Brand { get; set; }

        public Media? ThumbnailImage { get; set; }

        public List<ProductMedia> Medias { get; set; } = new();

        public List<ProductLink> ProductLinks { get; set; } = new();

        public List<ProductLink> LinkedProductLinks { get; set; } = new();

        public List<ProductAttributeValue> AttributeValues { get; set; } = new();

        public List<ProductOptionValue> OptionValues { get; set; } = new();

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

        public void AddProductLinks(ProductLink productLink)
        {
            productLink.Product = this;
            ProductLinks.Add(productLink);
        }
    }
}
