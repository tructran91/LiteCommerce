using Catalog.Application.Brands.Commands;
using Catalog.Application.Brands.Queries;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
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

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<BrandResponse>>), 200)]
        public async Task<IActionResult> GetAllBrands([FromQuery] GetAllBrandsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<BrandResponse>), 200)]
        public async Task<ActionResult> GetBrandById(string id)
        {
            var query = new GetBrandQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<BrandResponse>), 200)]
        public async Task<ActionResult> CreateBrand([FromBody] CreateBrandRequest request)
        {
            var command = new CreateBrandCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(BaseResponse<BrandResponse>), 200)]
        public async Task<IActionResult> UpdateBrand([FromBody] UpdateBrandRequest request)
        {
            var command = new UpdateBrandCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        public async Task<IActionResult> DeleteBrand(string id)
        {
            var command = new DeleteBrandCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
