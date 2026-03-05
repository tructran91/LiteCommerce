namespace Catalog.Application.Responses
{
    public class BasicProductResponse
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public bool IsFeatured { get; set; }

        public bool IsAllowToOrder { get; set; }

        public bool IsCallForPricing { get; set; }

        public bool IsPublished { get; set; }
    }
}
