using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using LiteCommerce.Shared.Authorization;
using LiteCommerce.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    // Read-only: activity rows are written by ActivityLogger from AuthService. Not behind the "auth" rate limiter.
    [Route("api/admin/activity-log")]
    [Authorize]
    public class AdminActivityLogController : BaseApiController
    {
        private readonly ILogQueryService _logs;

        public AdminActivityLogController(ILogQueryService logs)
        {
            _logs = logs;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ActivityLogResponse>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        [HasPermission(AppPermissions.IdentityLog.ViewActivity)]
        public async Task<IActionResult> GetAllActivityLogs([FromQuery] GetActivityLogsQuery query, CancellationToken cancellationToken)
            => ToActionResult(await _logs.GetActivityLogsAsync(query, cancellationToken));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ActivityLogResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<ActivityLogResponse>), 400)]
        [ProducesResponseType(typeof(BaseResponse<ActivityLogResponse>), 404)]
        [HasPermission(AppPermissions.IdentityLog.ViewActivity)]
        public async Task<IActionResult> GetActivityLogById(string id, CancellationToken cancellationToken)
            => ToActionResult(await _logs.GetActivityLogAsync(id, cancellationToken));
    }
}
