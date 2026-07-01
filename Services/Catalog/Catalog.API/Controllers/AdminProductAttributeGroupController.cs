using Catalog.Application.ProductAttributeGroups.Commands;
using Catalog.Application.ProductAttributeGroups.Queries;
using Catalog.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/admin/product-attribute-group")]
    [ApiController]
    public class AdminProductAttributeGroupController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminProductAttributeGroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllProductAttributeGroups([FromQuery] GetAllProductAttributeGroupsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetProductAttributeGroupById(string id)
        {
            var query = new GetProductAttributeGroupQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult> CreateProductAttributeGroup([FromBody] CreateProductAttributeGroupRequest request)
        {
            var command = new CreateProductAttributeGroupCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateProductAttributeGroup([FromBody] UpdateProductAttributeGroupRequest request)
        {
            var command = new UpdateProductAttributeGroupCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProductAttributeGroup(string id)
        {
            var command = new DeleteProductAttributeGroupCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
