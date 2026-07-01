namespace Catalog.Application.Requests
{
    public record CreateProductAttributeRequest
    {
        public string Name { get; set; }

        public string GroupId { get; set; }
    }
}
