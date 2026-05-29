namespace LiteCommerce.Admin.Models.Business.ProductPrice
{
    public class UpdateProductPricingRequest
    {
        public string Id { get; set; }

        public decimal? NewPrice { get; set; }

        public decimal? NewOldPrice { get; set; }
    }
}
