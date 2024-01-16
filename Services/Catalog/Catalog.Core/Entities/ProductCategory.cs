namespace Catalog.Core.Entities
{
    public class ProductCategory
    {
        public Guid Id { get; set; }

        public Guid CategoryId { get; set; }

        public Guid ProductId { get; set; }

        public Category Category { get; set; }

        public Product Product { get; set; }

        public int DisplayOrder { get; set; }
    }
}
