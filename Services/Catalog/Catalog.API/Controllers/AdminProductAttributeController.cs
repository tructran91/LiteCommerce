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
    public class AdminProductAttributeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminProductAttributeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ProductAttributeResponse>>), 200)]
        public async Task<ActionResult> GetAllProductAttributes([FromQuery] GetAllProductAttributesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ProductAttributeResponse>), 200)]
        public async Task<ActionResult> GetProductAttributeById(string id)
        {
            var query = new GetProductAttributeQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<ProductAttributeResponse>), 200)]
        public async Task<ActionResult> CreateProductAttribute([FromBody] CreateProductAttributeRequest request)
        {
            var command = new CreateProductAttributeCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(BaseResponse<ProductAttributeResponse>), 200)]
        public async Task<ActionResult> UpdateProductAttribute([FromBody] UpdateProductAttributeRequest request)
        {
            var command = new UpdateProductAttributeCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), 200)]
        public async Task<ActionResult> DeleteProductAttribute(string id)
        {
            var command = new DeleteProductAttributeCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
