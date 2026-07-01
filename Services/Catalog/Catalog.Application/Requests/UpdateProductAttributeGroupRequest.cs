namespace Catalog.Application.Requests
{
    public record UpdateProductAttributeGroupRequest : CreateProductAttributeGroupRequest
    {
        public string Id { get; set; }
    }
}
