namespace Catalog.Core.Entities
{
    // The value a variant takes on one axis. Exactly one row per axis declared on the
    // product, enforced by a unique index on (VariantId, OptionId).
    public class ProductVariantOptionValue : BaseEntity
    {
        public Guid VariantId { get; set; }

        public ProductVariant Variant { get; set; }

        public Guid OptionId { get; set; }

        public ProductOption Option { get; set; }

        public Guid ProductOptionValueItemId { get; set; }

        public ProductOptionValueItem ProductOptionValueItem { get; set; }

        public int SortIndex { get; set; }
    }
}
