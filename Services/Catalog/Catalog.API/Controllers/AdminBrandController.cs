using Catalog.Application.Brands.Commands;
using Catalog.Application.Brands.Queries;
using Catalog.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/admin/brand")]
    [ApiController]
    public class AdminBrandController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminBrandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllBrands")]
        public async Task<IActionResult> GetAllBrands([FromQuery] GetAllBrandsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetBrandById/{id}")]
        public async Task<ActionResult> GetBrandById(string id)
        {
            var query = new GetBrandQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("CreateBrand")]
        public async Task<ActionResult> CreateBrand([FromBody] CreateBrandRequest request)
        {
            var command = new CreateBrandCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("UpdateBrand")]
        public async Task<IActionResult> UpdateBrand([FromBody] UpdateBrandRequest request)
        {
            var command = new UpdateBrandCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
