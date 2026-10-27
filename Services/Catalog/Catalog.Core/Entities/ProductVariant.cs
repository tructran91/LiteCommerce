namespace Catalog.Core.Entities
{
    // Price, stock and sku live here and are the source of truth; name, description,
    // SEO, categories and attributes stay on Product and are shared by every variant.
    public class ProductVariant : BaseEntity
    {
        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        // Dash joined option value keys in axis order ("red-s"). The identity of a
        // combination, so variants are matched on it rather than on Name.
        public string NormalizedName { get; set; }

        public string? Name { get; set; }

        public string? Sku { get; set; }

        public string? Gtin { get; set; }

        public decimal Price { get; set; }

        public decimal? OldPrice { get; set; }

        public decimal? SpecialPrice { get; set; }

        public DateTime? SpecialPriceStart { get; set; }

        public DateTime? SpecialPriceEnd { get; set; }

        public int StockQuantity { get; set; }

        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; }

        public Media? ThumbnailImage { get; set; }

        public List<ProductVariantOptionValue> OptionValues { get; set; } = new();

        public void AddOptionValue(ProductVariantOptionValue optionValue)
        {
            optionValue.Variant = this;
            OptionValues.Add(optionValue);
        }
    }
}
