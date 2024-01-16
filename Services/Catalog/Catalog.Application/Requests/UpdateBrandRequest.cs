namespace Catalog.Application.Requests
{
    public class UpdateBrandRequest
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsPublished { get; set; }
    }
}
