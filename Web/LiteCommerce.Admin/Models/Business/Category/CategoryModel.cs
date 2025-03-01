﻿namespace LiteCommerce.Admin.Models.Business.Category
{
    public class CategoryModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Slug { get; set; }

        public string? Description { get; set; }

        public string? MetaTitle { get; set; }

        public string? MetaKeywords { get; set; }

        public string? MetaDescription { get; set; }

        public bool IsPublished { get; set; }

        public string DisplayName { get; set; }

        public bool IncludeInMenu { get; set; }

        public int DisplayOrder { get; set; }

        public string? ParentCategoryId { get; set; }
    }
}
