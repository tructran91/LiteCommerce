using Catalog.Application.ProductAttributes.Commands;
using Catalog.Application.ProductAttributes.Queries;
using Catalog.Application.Requests;
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

        [HttpGet("GetAllProductAttributes")]
        public async Task<ActionResult> GetAllProductAttributes([FromQuery] GetAllProductAttributesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetProductAttributeById/{id}")]
        public async Task<ActionResult> GetProductAttributeById(string id)
        {
            var query = new GetProductAttributeQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("CreateProductAttribute")]
        public async Task<ActionResult> CreateProductAttribute([FromBody] CreateProductAttributeRequest request)
        {
            var command = new CreateProductAttributeCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("UpdateProductAttribute")]
        public async Task<ActionResult> UpdateProductAttribute([FromBody] UpdateProductAttributeRequest request)
        {
            var command = new UpdateProductAttributeCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("DeleteProductAttribute/{id}")]
        public async Task<ActionResult> DeleteProductAttribute(string id)
        {
            var command = new DeleteProductAttributeCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
