namespace Catalog.Core.Entities
{
    public class ProductAttributeGroup
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public IList<ProductAttribute> Attributes { get; set; } = new List<ProductAttribute>();
    }
}
