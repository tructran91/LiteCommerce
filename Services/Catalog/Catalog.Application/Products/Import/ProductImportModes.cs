namespace Catalog.Application.Products.Import
{
    public static class ProductImportModes
    {
        public const string AllOrNothing = "AllOrNothing";
        public const string SkipInvalid = "SkipInvalid";

        public static bool TryParse(string? value, out ProductImportMode mode)
        {
            if (string.Equals(value, AllOrNothing, StringComparison.OrdinalIgnoreCase))
            {
                mode = ProductImportMode.AllOrNothing;
                return true;
            }

            if (string.Equals(value, SkipInvalid, StringComparison.OrdinalIgnoreCase))
            {
                mode = ProductImportMode.SkipInvalid;
                return true;
            }

            mode = ProductImportMode.AllOrNothing;
            return false;
        }

        // Validators already reject unknown modes, so handlers can parse without branching.
        public static ProductImportMode Parse(string? value)
            => TryParse(value, out var mode)
                ? mode
                : throw new ArgumentException($"Unknown import mode '{value}'.", nameof(value));
    }

    public enum ProductImportMode
    {
        AllOrNothing = 1,
        SkipInvalid
    }
}
