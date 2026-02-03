namespace Catalog.Core.Entities
{
    public class ProductTemplate : BaseEntity
    {
        public string Name { get; set; }

        public List<ProductTemplateProductAttribute> ProductAttributes { get; set; } = new();
    }
}
