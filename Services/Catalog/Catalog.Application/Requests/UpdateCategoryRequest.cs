namespace Catalog.Application.Requests
{
    public class UpdateCategoryRequest
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public string? MetaTitle { get; set; }

        public string? MetaKeywords { get; set; }

        public string? MetaDescription { get; set; }

        public bool IsPublished { get; set; }

        public bool IncludeInMenu { get; set; }

        public int DisplayOrder { get; set; }

        public string? ParentId { get; set; }
    }
}
