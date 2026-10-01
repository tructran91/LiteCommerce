using Catalog.Application.ActivityLogs.Queries;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    // Read-only: activity rows are written by ActivityLogBehavior.
    [Route("api/admin/activity-log")]
    [ApiController]
    public class AdminActivityLogController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdminActivityLogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ActivityLogResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllActivityLogs([FromQuery] GetAllActivityLogsQuery query)
        {
            var result = await _mediator.Send(query);
            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ActivityLogResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<ActivityLogResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetActivityLogById(string id)
        {
            var query = new GetActivityLogQuery(id);
            var result = await _mediator.Send(query);
            return ToActionResult(result);
        }
    }
}
