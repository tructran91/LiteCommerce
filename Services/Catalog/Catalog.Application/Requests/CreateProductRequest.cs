using Catalog.Application.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Requests
{
    public class CreateProductRequest
    {
        public ProductViewModel Product { get; set; } = new ProductViewModel();

        public IFormFile ThumbnailImage { get; set; }

        public List<IFormFile> ProductImages { get; set; } = new List<IFormFile>();

        public List<IFormFile> ProductDocuments { get; set; } = new List<IFormFile>();
    }
}
