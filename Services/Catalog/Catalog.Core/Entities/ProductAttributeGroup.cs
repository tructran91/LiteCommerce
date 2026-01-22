namespace Catalog.Core.Entities
{
    public class ProductAttributeGroup : BaseEntity
    {
        public string Name { get; set; }

        public List<ProductAttribute> Attributes { get; set; } = new();
    }
}
