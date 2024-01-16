namespace Catalog.Core.Entities
{
    public class ProductPriceHistory : BaseEntity
    {
        public Product Product { get; set; }

        public decimal? Price { get; set; }

        public decimal? OldPrice { get; set; }

        public decimal? SpecialPrice { get; set; }

        public DateTime? SpecialPriceStart { get; set; }

        public DateTime? SpecialPriceEnd { get; set; }
    }
}
