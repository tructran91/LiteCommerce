namespace Catalog.Core.Entities
{
    public class ProductOptionValue : BaseEntity
    {
        public Guid OptionId { get; set; }

        public ProductOption Option { get; set; }

        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        public string Value { get; set; }

        public string DisplayType { get; set; }

        public int SortIndex { get; set; }
    }
}
