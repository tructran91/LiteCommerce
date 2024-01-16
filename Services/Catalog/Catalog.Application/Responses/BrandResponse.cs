namespace Catalog.Application.Responses
{
    public class BrandResponse
    {
        public Guid? Id { get; set; }

        public string Name { get; set; }

        public string? Slug { get; set; }

        public bool IsPublished { get; set; }
    }
}
