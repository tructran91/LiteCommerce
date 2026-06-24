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

            var provider = _config["Storage:Provider"];
            if (provider != "Local")
                return BadRequest("File serving is not available for the current storage provider.");

            var rootPath = _config["Storage:Local:Path"];

            // Remove leading slash và normalize path
            fileName = fileName.TrimStart('/', '\\').Replace("/", Path.DirectorySeparatorChar.ToString());
            var fullPath = Path.Combine(rootPath, fileName);
            var normalizedFullPath = Path.GetFullPath(fullPath);
            var normalizedRootPath = Path.GetFullPath(rootPath);

            // Validate path không thoát khỏi rootPath (security)
            if (!normalizedFullPath.StartsWith(normalizedRootPath, StringComparison.OrdinalIgnoreCase))
                return BadRequest("Invalid path");

            if (!System.IO.File.Exists(normalizedFullPath))
                return NotFound();

            var contentTypeProvider = new FileExtensionContentTypeProvider();
            contentTypeProvider.TryGetContentType(normalizedFullPath, out var contentType);

            return PhysicalFile(normalizedFullPath, contentType ?? "application/octet-stream");
        }
    }
}
