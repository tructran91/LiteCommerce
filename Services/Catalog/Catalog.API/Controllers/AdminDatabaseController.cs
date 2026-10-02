using Catalog.Application.Database.Commands;
using Catalog.Application.Database.Queries;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/admin/database")]
    [ApiController]
    public class AdminDatabaseController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdminDatabaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("seed-profiles")]
        [ProducesResponseType(typeof(BaseResponse<List<string>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetSeedProfiles()
        {
            var result = await _mediator.Send(new GetSeedProfilesQuery());
            return ToActionResult(result);
        }

        [HttpPost("seed")]
        [ProducesResponseType(typeof(BaseResponse<SeedDatabaseResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SeedDatabase([FromBody] SeedDatabaseRequest request)
        {
            var command = new SeedDatabaseCommand(request);
            var result = await _mediator.Send(command);
            return ToActionResult(result);
        }
    }
}
