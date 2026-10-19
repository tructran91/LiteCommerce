using Identity.Application.DTOs;
using Identity.Application.Interfaces;
using LiteCommerce.Shared.Authorization;
using LiteCommerce.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    // Read-only: audit rows are written by AuditLogInterceptor. Not behind the "auth" rate limiter.
    [Route("api/admin/audit-log")]
    [Authorize(Policy = AppPolicies.AdminOnly)]
    public class AdminAuditLogController : BaseApiController
    {
        private readonly ILogQueryService _logs;

        public AdminAuditLogController(ILogQueryService logs)
        {
            _logs = logs;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<AuditLogResponse>>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        public async Task<IActionResult> GetAllAuditLogs([FromQuery] GetAuditLogsQuery query, CancellationToken cancellationToken)
            => ToActionResult(await _logs.GetAuditLogsAsync(query, cancellationToken));

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<AuditLogResponse>), 200)]
        [ProducesResponseType(typeof(BaseResponse<AuditLogResponse>), 400)]
        [ProducesResponseType(typeof(BaseResponse<AuditLogResponse>), 404)]
        public async Task<IActionResult> GetAuditLogById(string id, CancellationToken cancellationToken)
            => ToActionResult(await _logs.GetAuditLogAsync(id, cancellationToken));
    }
}
