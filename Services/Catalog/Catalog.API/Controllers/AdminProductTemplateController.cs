using Catalog.Application.ProductTemplates.Commands;
using Catalog.Application.ProductTemplates.Queries;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/admin/product-template")]
    [ApiController]
    public class AdminProductTemplateController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdminProductTemplateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ProductTemplateResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllProductTemplates([FromQuery] GetAllProductTemplatesQuery query)
        {
            var result = await _mediator.Send(query);
            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ProductTemplateResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<ProductTemplateResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductTemplateById(string id)
        {
            var query = new GetProductTemplateQuery(id);
            var result = await _mediator.Send(query);
            return ToActionResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<ProductTemplateResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<ProductTemplateResponse>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateProductTemplate([FromBody] CreateProductTemplateRequest request)
        {
            var command = new CreateProductTemplateCommand(request);
            var result = await _mediator.Send(command);
            return ToCreatedResult(result, nameof(GetProductTemplateById), item => new { id = item.Id });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ProductTemplateResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<ProductTemplateResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<ProductTemplateResponse>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateProductTemplate(string id, [FromBody] UpdateProductTemplateRequest request)
        {
            var command = new UpdateProductTemplateCommand(id, request);
            var result = await _mediator.Send(command);
            return ToActionResult(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteProductTemplate(string id)
        {
            var command = new DeleteProductTemplateCommand(id);
            var result = await _mediator.Send(command);
            return ToActionResult(result);
        }
    }
}
