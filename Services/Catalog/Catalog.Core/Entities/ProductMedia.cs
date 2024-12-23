namespace Catalog.Core.Entities
{
    public class ProductMedia : BaseEntity
    {
        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        public Guid MediaId { get; set; }

        public Media Media { get; set; }
    }
}
