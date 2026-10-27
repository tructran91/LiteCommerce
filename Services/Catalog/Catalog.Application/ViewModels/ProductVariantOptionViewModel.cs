namespace Catalog.Application.ViewModels
{
    // OptionId plus ValueKey is what the server resolves against the product's declared
    // options. OptionName and ValueDisplay are response-only labels.
    public class ProductVariantOptionViewModel
    {
        public string OptionId { get; set; }

        public string? OptionName { get; set; }

        public string ValueKey { get; set; }

        public string? ValueDisplay { get; set; }

        public int SortIndex { get; set; }
    }
}
