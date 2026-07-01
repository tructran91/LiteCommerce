namespace Catalog.Application.Requests
{
    public record UpdateProductPricingRequest
    {
        public string Id { get; set; }

        public decimal? NewPrice { get; set; }

        public decimal? NewOldPrice { get; set; }
    }
}
