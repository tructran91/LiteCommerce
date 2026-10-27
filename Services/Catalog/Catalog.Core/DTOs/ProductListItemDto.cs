namespace Catalog.Core.DTOs
{
    public class ProductListItemDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public bool HasVariants { get; set; }

        public int VariantCount { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsAllowToOrder { get; set; }

        public bool IsCallForPricing { get; set; }

        public bool IsPublished { get; set; }
    }
}
