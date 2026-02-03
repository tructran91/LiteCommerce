using Catalog.Application.ProductTemplates.Commands;
using Catalog.Application.ProductTemplates.Queries;
using Catalog.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/admin/product-template")]
    [ApiController]
    public class AdminProductTemplateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminProductTemplateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllProductTemplates")]
        public async Task<ActionResult> GetAllProductTemplates([FromQuery] GetAllProductTemplatesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetProductTemplateById/{id}")]
        public async Task<ActionResult> GetProductTemplateById(string id)
        {
            var query = new GetProductTemplateQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("CreateProductTemplate")]
        public async Task<ActionResult> CreateProductTemplate([FromBody] CreateProductTemplateRequest request)
        {
            var command = new CreateProductTemplateCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("UpdateProductTemplate")]
        public async Task<ActionResult> UpdateProductTemplate([FromBody] UpdateProductTemplateRequest request)
        {
            var command = new UpdateProductTemplateCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("DeleteProductTemplate/{id}")]
        public async Task<ActionResult> DeleteProductTemplate(string id)
        {
            var command = new DeleteProductTemplateCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
