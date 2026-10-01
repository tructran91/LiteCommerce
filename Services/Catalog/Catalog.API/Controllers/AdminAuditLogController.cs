using Catalog.Application.AuditLogs.Queries;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    // Read-only: audit rows are written by AuditLogInterceptor.
    [Route("api/admin/audit-log")]
    [ApiController]
    public class AdminAuditLogController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdminAuditLogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<AuditLogResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllAuditLogs([FromQuery] GetAllAuditLogsQuery query)
        {
            var result = await _mediator.Send(query);
            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<AuditLogResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<AuditLogResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAuditLogById(string id)
        {
            var query = new GetAuditLogQuery(id);
            var result = await _mediator.Send(query);
            return ToActionResult(result);
        }
    }
}
