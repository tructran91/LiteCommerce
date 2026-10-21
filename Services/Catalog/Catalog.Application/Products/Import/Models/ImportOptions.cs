namespace Catalog.Application.Products.Import.Models
{
    public class ImportOptions
    {
        public bool CreateMissingBrands { get; set; }

        public ProductImportMode Mode { get; set; } = ProductImportMode.AllOrNothing;
    }
}
