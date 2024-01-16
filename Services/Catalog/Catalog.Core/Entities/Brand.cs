namespace Catalog.Core.Entities
{
    public class Brand : BaseEntity
    {
        public string Name { get; set; }

        public string Slug { get; set; }

        public bool IsPublished { get; set; }
    }
}
