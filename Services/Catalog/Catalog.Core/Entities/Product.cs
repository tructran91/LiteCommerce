namespace Catalog.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }

        public string Slug { get; set; }

        public string? Description { get; set; }

        public string? MetaTitle { get; set; }

        public string? MetaKeywords { get; set; }

        public string? MetaDescription { get; set; }

        public bool IsPublished { get; set; }

        public string? ShortDescription { get; set; }

        public decimal Price { get; set; } = 0;

        public decimal? OldPrice { get; set; }

        public decimal? SpecialPrice { get; set; }

        public DateTime? SpecialPriceStart { get; set; }

        public DateTime? SpecialPriceEnd { get; set; }

        public bool IsCallForPricing { get; set; }

        public bool IsAllowToOrder { get; set; }

        public int StockQuantity { get; set; }

        public string? Sku { get; set; }

        public int DisplayOrder { get; set; }

        public Guid BrandId { get; set; }

        public Brand Brand { get; set; }

        public Media? ThumbnailImage { get; set; }

        public IList<ProductMedia> Medias { get; set; } = new List<ProductMedia>();

        public IList<ProductLink> ProductLinks { get; set; } = new List<ProductLink>();

        public IList<ProductLink> LinkedProductLinks { get; set; } = new List<ProductLink>();

        public IList<ProductAttributeValue> AttributeValues { get; set; } = new List<ProductAttributeValue>();

        //public IList<ProductOptionValue> OptionValues { get; set; } = new List<ProductOptionValue>();

        public IList<ProductCategory> Categories { get; set; } = new List<ProductCategory>();

        //public IList<ProductPriceHistory> PriceHistories { get; set; } = new List<ProductPriceHistory>();
    }
}
