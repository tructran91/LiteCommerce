using Catalog.Core.Enums;

namespace Catalog.Core.Entities
{
    public class ProductLink : BaseEntity
    {
        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        public Guid LinkedProductId { get; set; }

        public Product LinkedProduct { get; set; }

        public ProductLinkType LinkType { get; set; }
    }
}
