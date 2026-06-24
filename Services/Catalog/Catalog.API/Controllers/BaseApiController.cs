using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected string? BuildImageUrl(string? url)
        {
            if (string.IsNullOrEmpty(url))
                return url;

            if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                return url;

            return $"{Request.Scheme}://{Request.Host}/api/public/files{url}";
        }
    }
}
