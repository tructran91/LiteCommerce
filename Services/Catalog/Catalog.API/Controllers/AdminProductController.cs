using Catalog.Application.Products.Commands;
using Catalog.Application.Products.Queries;
using Catalog.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/admin/product")]
    [ApiController]
    public class AdminProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetProductById/{id}")]
        public async Task<ActionResult> GetBrandById(string id)
        {
            var query = new GetProductQuery(Guid.Parse(id));
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("CreateProduct")]
        public async Task<ActionResult> CreateProduct([FromForm] CreateProductRequest request)
        {
            var command = new CreateProductCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
