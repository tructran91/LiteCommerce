using Catalog.Application.ProductOptions.Commands;
using Catalog.Application.ProductOptions.Queries;
using Catalog.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/admin/product-option")]
    [ApiController]
    public class AdminProductOptionController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminProductOptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllProductOptions")]
        public async Task<ActionResult> GetAllProductOptions([FromQuery] GetAllProductOptionsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetProductOptionById/{id}")]
        public async Task<ActionResult> GetProductOptionById(string id)
        {
            var query = new GetProductOptionQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("CreateProductOption")]
        public async Task<ActionResult> CreateProductOption([FromBody] CreateProductOptionRequest request)
        {
            var command = new CreateProductOptionCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("UpdateProductOption")]
        public async Task<ActionResult> UpdateProductOption([FromBody] UpdateProductOptionRequest request)
        {
            var command = new UpdateProductOptionCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("DeleteProductOption/{id}")]
        public async Task<ActionResult> DeleteProductOption(string id)
        {
            var command = new DeleteProductOptionCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
