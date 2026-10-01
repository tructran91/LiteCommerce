using Catalog.Application.ProductAttributeGroups.Commands;
using Catalog.Application.ProductAttributeGroups.Queries;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/admin/product-attribute-group")]
    [ApiController]
    public class AdminProductAttributeGroupController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdminProductAttributeGroupController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ProductAttributeGroupResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllProductAttributeGroups([FromQuery] GetAllProductAttributeGroupsQuery query)
        {
            var result = await _mediator.Send(query);
            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ProductAttributeGroupResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<ProductAttributeGroupResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductAttributeGroupById(string id)
        {
            var query = new GetProductAttributeGroupQuery(id);
            var result = await _mediator.Send(query);
            return ToActionResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<ProductAttributeGroupResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<ProductAttributeGroupResponse>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateProductAttributeGroup([FromBody] CreateProductAttributeGroupRequest request)
        {
            var command = new CreateProductAttributeGroupCommand(request);
            var result = await _mediator.Send(command);
            return ToCreatedResult(result, nameof(GetProductAttributeGroupById), item => new { id = item.Id });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ProductAttributeGroupResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<ProductAttributeGroupResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<ProductAttributeGroupResponse>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateProductAttributeGroup(string id, [FromBody] UpdateProductAttributeGroupRequest request)
        {
            var command = new UpdateProductAttributeGroupCommand(id, request);
            var result = await _mediator.Send(command);
            return ToActionResult(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteProductAttributeGroup(string id)
        {
            var command = new DeleteProductAttributeGroupCommand(id);
            var result = await _mediator.Send(command);
            return ToActionResult(result);
        }
    }
}
