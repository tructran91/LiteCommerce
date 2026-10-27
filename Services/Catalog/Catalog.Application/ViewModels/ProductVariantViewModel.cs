namespace Catalog.Application.ViewModels
{
    public class ProductVariantViewModel
    {
        // Empty on a newly generated variant, set on an existing one.
        public string? Id { get; set; }

        public string? Name { get; set; }

        // Response only. Derived from OptionCombinations, never read from the client.
        public string? NormalizedName { get; set; }

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

        public string? ThumbnailImageUrl { get; set; }

        public IList<ProductVariantOptionViewModel> OptionCombinations { get; set; }
            = new List<ProductVariantOptionViewModel>();
    }
}
