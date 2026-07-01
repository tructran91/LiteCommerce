namespace LiteCommerce.Admin.Models.Business.ProductPrice
{
    public class UpdateProductPricingListRequest
    {
        public List<UpdateProductPricingRequest> Items { get; set; } = new();
    }
}
