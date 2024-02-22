namespace Catalog.Core.Entities
{
    public class ProductAttribute : BaseEntity
    {
        public string Name { get; set; }

        public Guid GroupId { get; set; }

        public ProductAttributeGroup Group { get; set; }
    }
}
