using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace Catalog.API.Controllers
{
    [Route("api/public/files")]
    [ApiController]
    public class PublicFilesController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PublicFilesController(IConfiguration config)
        {
            _config = config;
        }

        [HttpGet("{*fileName}")]
        public IActionResult GetFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return BadRequest();

            fileName = Path.GetFileName(fileName);

            var rootPath = _config["Storage:LocalPath"];
            var fullPath = Path.Combine(rootPath, fileName);

            if (!System.IO.File.Exists(fullPath))
                return NotFound();

            var provider = new FileExtensionContentTypeProvider();
            provider.TryGetContentType(fullPath, out var contentType);

            return PhysicalFile(fullPath,contentType ?? "application/octet-stream");
        }
    }
}
