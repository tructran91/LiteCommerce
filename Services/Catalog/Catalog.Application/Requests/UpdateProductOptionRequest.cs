namespace Catalog.Application.Requests
{
    public record UpdateProductOptionRequest : CreateProductOptionRequest
    {
        public string Id { get; set; }
    }
}
