using LiteCommerce.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    public abstract class BaseApiController : ControllerBase
    {
        protected IActionResult ToActionResult<T>(BaseResponse<T> result)
            => StatusCode((int)result.StatusCode, result);
    }
}
