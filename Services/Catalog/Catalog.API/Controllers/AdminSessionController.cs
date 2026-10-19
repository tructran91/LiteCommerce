using Catalog.Application.Responses;
using LiteCommerce.Shared.Authorization;
using LiteCommerce.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    // Lets the admin client verify right after login that this API accepts the token, so a
    // non-Admin user is rejected with the API's message instead of a client-side guess.
    [Route("api/admin/session")]
    [ApiController]
    public class AdminSessionController : BaseApiController
    {
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<SessionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status403Forbidden)]
        public IActionResult GetSession()
        {
            var session = new SessionResponse
            {
                UserName = User.Identity?.Name ?? string.Empty,
                Role = User.FindFirst(AppClaimTypes.Role)?.Value ?? string.Empty
            };

            return ToActionResult(BaseResponse<SessionResponse>.Success(session));
        }
    }
}
