namespace Catalog.Core.Entities
{
    public class ProductCategory : BaseEntity
    {
        public Guid CategoryId { get; set; }

        public Guid ProductId { get; set; }

        public Category Category { get; set; }

        public Product Product { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsFeaturedProduct { get; set; }
    }
}
