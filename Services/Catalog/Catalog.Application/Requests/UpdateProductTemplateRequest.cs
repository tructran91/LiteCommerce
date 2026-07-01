namespace Catalog.Application.Requests
{
    public record UpdateProductTemplateRequest : CreateProductTemplateRequest
    {
        public string Id { get; set; }
    }
}
