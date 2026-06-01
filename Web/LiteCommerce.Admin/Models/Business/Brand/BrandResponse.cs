namespace LiteCommerce.Admin.Models.Business.Brand
{
    public class BrandResponse
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string? Slug { get; set; }

        public bool IsPublished { get; set; }
    }
}
