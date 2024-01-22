using Catalog.Application.Brands.Commands;
using Catalog.Application.Brands.Queries;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        [ProducesResponseType(typeof(BaseResponse<List<BrandResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllBrands()
        {
            var query = new GetAllBrandsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetBrandById/{id}")]
        [ProducesResponseType(typeof(BaseResponse<BrandResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetBrandById(string id)
        {
            var query = new GetBrandQuery(Guid.Parse(id));
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("CreateBrand")]
        [ProducesResponseType(typeof(BaseResponse<BrandResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateBrand([FromBody] CreateBrandRequest request)
        {
            var command = new CreateBrandCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateBrand")]
        [ProducesResponseType(typeof(BaseResponse<BrandResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> UpdateBrand([FromBody] UpdateBrandRequest request)
        {
            var command = new UpdateBrandCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
