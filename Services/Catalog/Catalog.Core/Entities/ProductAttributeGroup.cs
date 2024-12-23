namespace Catalog.Core.Entities
{
    public class ProductAttributeGroup
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<ProductAttribute> Attributes { get; set; } = new();
    }
}
