using Catalog.Application.ProductAttributes.Commands;
using Catalog.Application.ProductAttributes.Queries;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/admin/product-attribute")]
    [ApiController]
    public class AdminProductAttributeController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdminProductAttributeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ProductAttributeResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllProductAttributes([FromQuery] GetAllProductAttributesQuery query)
        {
            var result = await _mediator.Send(query);
            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ProductAttributeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<ProductAttributeResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductAttributeById(string id)
        {
            var query = new GetProductAttributeQuery(id);
            var result = await _mediator.Send(query);
            return ToActionResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<ProductAttributeResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<ProductAttributeResponse>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateProductAttribute([FromBody] CreateProductAttributeRequest request)
        {
            var command = new CreateProductAttributeCommand(request);
            var result = await _mediator.Send(command);
            return ToCreatedResult(result, nameof(GetProductAttributeById), item => new { id = item.Id });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ProductAttributeResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<ProductAttributeResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<ProductAttributeResponse>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateProductAttribute(string id, [FromBody] UpdateProductAttributeRequest request)
        {
            var command = new UpdateProductAttributeCommand(id, request);
            var result = await _mediator.Send(command);
            return ToActionResult(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteProductAttribute(string id)
        {
            var command = new DeleteProductAttributeCommand(id);
            var result = await _mediator.Send(command);
            return ToActionResult(result);
        }
    }
}
