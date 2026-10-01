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
    public class AdminBrandController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdminBrandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<BrandResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllBrands([FromQuery] GetAllBrandsQuery query)
        {
            var result = await _mediator.Send(query);
            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<BrandResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<BrandResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBrandById(string id)
        {
            var query = new GetBrandQuery(id);
            var result = await _mediator.Send(query);
            return ToActionResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<BrandResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<BrandResponse>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateBrand([FromBody] CreateBrandRequest request)
        {
            var command = new CreateBrandCommand(request);
            var result = await _mediator.Send(command);
            return ToCreatedResult(result, nameof(GetBrandById), brand => new { id = brand.Id });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseResponse<BrandResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<BrandResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<BrandResponse>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateBrand(string id, [FromBody] UpdateBrandRequest request)
        {
            var command = new UpdateBrandCommand(id, request);
            var result = await _mediator.Send(command);
            return ToActionResult(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteBrand(string id)
        {
            var command = new DeleteBrandCommand(id);
            var result = await _mediator.Send(command);
            return ToActionResult(result);
        }
    }
}
