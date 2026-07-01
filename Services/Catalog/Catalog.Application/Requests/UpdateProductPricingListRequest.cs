namespace Catalog.Application.Requests
{
    public class UpdateProductPricingListRequest
    {
        public List<UpdateProductPricingRequest> Items { get; set; } = new();
    }
}
