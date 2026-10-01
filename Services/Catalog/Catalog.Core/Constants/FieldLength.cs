namespace Catalog.Core.Constants
{
    // Shared by EF configurations and validators. SQL Server cannot index nvarchar(max).
    public static class FieldLength
    {
        public const int BrandName = 255;
        public const int BrandSlug = 255;

        public const int CategoryName = 255;
        public const int CategorySlug = 255;

        public const int ProductName = 450;
        public const int ProductSlug = 450;
    }
}
