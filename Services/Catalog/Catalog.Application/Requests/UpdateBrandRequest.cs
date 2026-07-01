namespace Catalog.Application.Requests
{
    public record UpdateBrandRequest : CreateBrandRequest
    {
        public string Id { get; set; }
    }
}
