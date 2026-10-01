using LiteCommerce.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult ToActionResult<T>(BaseResponse<T> result)
            => StatusCode((int)result.StatusCode, result);

        protected IActionResult ToCreatedResult<T>(BaseResponse<T> result, string actionName, Func<T, object> routeValues)
            => result.IsSuccess
                ? CreatedAtAction(actionName, routeValues(result.Data), result)
                : ToActionResult(result);

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
