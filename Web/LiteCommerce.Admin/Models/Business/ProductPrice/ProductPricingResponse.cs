namespace LiteCommerce.Admin.Models.Business.ProductPrice
{
    public class ProductPricingResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; } = 0;

        public decimal? OldPrice { get; set; }

        public decimal? SpecialPrice { get; set; }

        public DateTime? SpecialPriceStart { get; set; }

        public DateTime? SpecialPriceEnd { get; set; }

        // Track original values to detect changes
        public decimal OriginalPrice { get; set; } = 0;

        public decimal? OriginalOldPrice { get; set; }

        public bool HasPriceChanged => Price != OriginalPrice;

        public bool HasOldPriceChanged => OldPrice != OriginalOldPrice;

        public bool HasAnyChange => HasPriceChanged || HasOldPriceChanged;
    }
}
