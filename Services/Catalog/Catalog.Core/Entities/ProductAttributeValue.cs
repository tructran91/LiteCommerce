namespace Catalog.Core.Entities
{
    public class ProductAttributeValue
    {
        public Guid Id { get; set; }

        public Guid AttributeId { get; set; }

        public Guid ProductId { get; set; }

        public string Value { get; set; }

        public ProductAttribute Attribute { get; set; }

        public Product Product { get; set; }
    }
}
