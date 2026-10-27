namespace LiteCommerce.Admin.Models.Business.Product
{
    public class ProductVariantFormItem
    {
        // Empty until the row round-trips from the server. Keeping it is what makes the
        // server update an existing variant rather than replace it.
        public string Id { get; set; }

        public string Name { get; set; }

        // Server derived; shown but never sent as a source of truth.
        public string NormalizedName { get; set; }

        public string Sku { get; set; }

        public string Gtin { get; set; }

        public decimal Price { get; set; }

        public decimal? OldPrice { get; set; }

        public decimal? SpecialPrice { get; set; }

        public DateTime? SpecialPriceStart { get; set; }

        public DateTime? SpecialPriceEnd { get; set; }

        public int StockQuantity { get; set; }

        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; }

        public string ThumbnailImageUrl { get; set; }

        public IList<ProductVariantOptionFormItem> OptionCombinations { get; set; }
            = new List<ProductVariantOptionFormItem>();

        public string CombinationLabel =>
            string.Join(" / ", OptionCombinations.Select(c => c.ValueDisplay ?? c.ValueKey));
    }
}
