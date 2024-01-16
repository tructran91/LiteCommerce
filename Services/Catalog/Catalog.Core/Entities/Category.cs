namespace Catalog.Core.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }

        public string Slug { get; set; }

        public string? Description { get; set; }

        public string? MetaTitle { get; set; }

        public string? MetaKeywords { get; set; }

        public string? MetaDescription { get; set; }

        public bool IsPublished { get; set; }

        public bool IncludeInMenu { get; set; }

        public int DisplayOrder { get; set; }

        public Guid? ParentId { get; set; }

        public Category? Parent { get; set; }

        public IList<Category> Children { get; set; } = new List<Category>();
    }
}
