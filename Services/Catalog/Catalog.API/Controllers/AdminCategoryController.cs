using Catalog.Application.Categories.Commands;
using Catalog.Application.Categories.Queries;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/admin/category")]
    [ApiController]
    public class AdminCategoryController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdminCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<CategoryResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllCategories([FromQuery] GetAllCategoriesQuery query)
        {
            var result = await _mediator.Send(query);
            return ToActionResult(result);
        }

        [HttpGet("basic")]
        [ProducesResponseType(typeof(BaseResponse<List<BasicCategoryResponse>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllBasicCategories()
        {
            var query = new GetAllBasicCategoriesQuery();
            var result = await _mediator.Send(query);
            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<CategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<CategoryResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            var query = new GetCategoryQuery(id);
            var result = await _mediator.Send(query);
            ApplyImageUrl(result);
            return ToActionResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<CategoryResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<CategoryResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<CategoryResponse>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateCategory([FromForm] CreateCategoryRequest request)
        {
            var command = new CreateCategoryCommand(request);
            var result = await _mediator.Send(command);
            ApplyImageUrl(result);
            return ToCreatedResult(result, nameof(GetCategoryById), category => new { id = category.Id });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseResponse<CategoryResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<CategoryResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<CategoryResponse>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateCategory(string id, [FromForm] UpdateCategoryRequest request)
        {
            var command = new UpdateCategoryCommand(id, request);
            var result = await _mediator.Send(command);
            ApplyImageUrl(result);
            return ToActionResult(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var command = new DeleteCategoryCommand(id);
            var result = await _mediator.Send(command);
            return ToActionResult(result);
        }

        private void ApplyImageUrl(BaseResponse<CategoryResponse> result)
        {
            if (result.IsSuccess && result.Data != null)
            {
                result.Data.ThumbnailImageUrl = BuildImageUrl(result.Data.ThumbnailImageUrl);
            }
        }
    }
}
