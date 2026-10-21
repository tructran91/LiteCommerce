using Catalog.Application.Products.Import;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Requests
{
    // Contains IFormFile, so it must stay a class (records cannot bind multipart files reliably).
    public class ImportProductsRequest
    {
        public IFormFile? File { get; set; }

        public bool CreateMissingBrands { get; set; }

        public string Mode { get; set; } = ProductImportModes.AllOrNothing;
    }
}
