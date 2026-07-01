namespace Catalog.Application.Requests
{
    public record CreateBrandRequest
    {
        public string Name { get; set; }

        public bool IsPublished { get; set; }
    }
}
