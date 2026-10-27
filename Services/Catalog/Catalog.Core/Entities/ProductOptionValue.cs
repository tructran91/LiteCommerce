namespace Catalog.Core.Entities
{
    // Declares that a product uses a given option axis. The allowed values are
    // ProductOptionValueItem rows.
    public class ProductOptionValue : BaseEntity
    {
        public Guid OptionId { get; set; }

        public ProductOption Option { get; set; }

        public Guid ProductId { get; set; }

        public Product Product { get; set; }

        // "text", "color" or "image".
        public string DisplayType { get; set; }

        // Orders the segments of ProductVariant.NormalizedName, so reordering axes
        // re-identifies every existing variant.
        public int SortIndex { get; set; }

        public List<ProductOptionValueItem> Items { get; set; } = new();

        public void AddItem(ProductOptionValueItem item)
        {
            item.ProductOptionValue = this;
            Items.Add(item);
        }
    }
}
