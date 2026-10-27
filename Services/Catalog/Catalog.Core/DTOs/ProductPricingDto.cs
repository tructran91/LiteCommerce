namespace Catalog.Core.DTOs
{
    public class ProductPricingDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        // Lets the pricing grid disable the row: prices of a product with variants are
        // derived and must be edited on each variant instead.
        public bool HasVariants { get; set; }

        public decimal Price { get; set; } = 0;

        public decimal PriceMax { get; set; } = 0;

        public decimal? OldPrice { get; set; }

        public decimal? SpecialPrice { get; set; }

        public DateTime? SpecialPriceStart { get; set; }

        public DateTime? SpecialPriceEnd { get; set; }
    }
}
