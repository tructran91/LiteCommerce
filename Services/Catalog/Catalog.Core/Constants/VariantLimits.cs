namespace Catalog.Core.Constants
{
    // Caps the cartesian product so one multipart request cannot build thousands of
    // variants. Enforced by the validators and mirrored in the Admin UI.
    public static class VariantLimits
    {
        public const int MaxOptionsPerProduct = 3;

        public const int MaxValuesPerOption = 50;

        public const int MaxVariantsPerProduct = 200;
    }
}
