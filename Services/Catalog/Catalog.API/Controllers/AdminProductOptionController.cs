using Catalog.Application.ProductOptions.Commands;
using Catalog.Application.ProductOptions.Queries;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/admin/product-option")]
    [ApiController]
    public class AdminProductOptionController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdminProductOptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ProductOptionResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllProductOptions([FromQuery] GetAllProductOptionsQuery query)
        {
            var result = await _mediator.Send(query);
            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ProductOptionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<ProductOptionResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductOptionById(string id)
        {
            var query = new GetProductOptionQuery(id);
            var result = await _mediator.Send(query);
            return ToActionResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<ProductOptionResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<ProductOptionResponse>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateProductOption([FromBody] CreateProductOptionRequest request)
        {
            var command = new CreateProductOptionCommand(request);
            var result = await _mediator.Send(command);
            return ToCreatedResult(result, nameof(GetProductOptionById), item => new { id = item.Id });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ProductOptionResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<ProductOptionResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<ProductOptionResponse>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateProductOption(string id, [FromBody] UpdateProductOptionRequest request)
        {
            var command = new UpdateProductOptionCommand(id, request);
            var result = await _mediator.Send(command);
            return ToActionResult(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteProductOption(string id)
        {
            var command = new DeleteProductOptionCommand(id);
            var result = await _mediator.Send(command);
            return ToActionResult(result);
        }
    }
}
