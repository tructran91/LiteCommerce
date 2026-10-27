namespace Catalog.Core.Entities
{
    // One allowed value of an axis on one product ("Red" on Color). A table rather than a
    // JSON column so variants can hold a foreign key and the storefront can filter on it.
    public class ProductOptionValueItem : BaseEntity
    {
        public Guid ProductOptionValueId { get; set; }

        public ProductOptionValue ProductOptionValue { get; set; }

        // Feeds ProductVariant.NormalizedName, so changing it re-identifies every
        // variant that uses this value. Display is only a label and is safe to rename.
        public string Key { get; set; }

        public string Display { get; set; }

        public int SortIndex { get; set; }

        public List<ProductVariantOptionValue> VariantOptionValues { get; set; } = new();
    }
}
