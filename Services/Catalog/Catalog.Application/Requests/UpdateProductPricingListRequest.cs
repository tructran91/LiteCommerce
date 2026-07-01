namespace Catalog.Application.Requests
{
    public record UpdateProductPricingListRequest
    {
        public List<UpdateProductPricingRequest> Items { get; set; } = new();
    }
}
