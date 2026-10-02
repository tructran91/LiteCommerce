namespace Catalog.Application.Responses
{
    public record SeedDatabaseResponse
    {
        public string Profile { get; set; }

        public int BrandCount { get; set; }

        public int CategoryCount { get; set; }

        public int ProductCount { get; set; }
    }
}
