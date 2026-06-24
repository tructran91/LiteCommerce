using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Requests
{
    public class UploadContentImageRequest
    {
        public IFormFile File { get; set; }

        public string ProductId { get; set; }

        public bool IsNewProduct { get; set; } = false;
    }
}
