namespace Catalog.Application.Requests
{
    public record UpdateProductAttributeRequest : CreateProductAttributeRequest
    {
        public string Id { get; set; }
    }
}
