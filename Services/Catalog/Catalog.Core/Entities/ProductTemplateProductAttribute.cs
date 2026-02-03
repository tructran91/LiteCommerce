namespace Catalog.Core.Entities
{
    public class ProductTemplateProductAttribute : BaseEntity
    {
        public Guid ProductTemplateId { get; set; }

        public ProductTemplate ProductTemplate { get; set; }

        public Guid ProductAttributeId { get; set; }

        public ProductAttribute ProductAttribute { get; set; }
    }
}
