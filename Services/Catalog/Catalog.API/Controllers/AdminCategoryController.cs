using Catalog.Application.Brands.Commands;
using Catalog.Application.Brands.Queries;
using Catalog.Application.Categories.Commands;
using Catalog.Application.Categories.Queries;
using Catalog.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/admin/category")]
    [ApiController]
    public class AdminCategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminCategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories([FromQuery] GetAllCategoriesQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetAllBasicCategories")]
        public async Task<IActionResult> GetAllBasicCategories()
        {
            var query = new GetAllBasicCategoriesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetCategoryById/{id}")]
        public async Task<ActionResult> GetCategoryById(string id)
        {
            var query = new GetCategoryQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("CreateCategory")]
        public async Task<ActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            var command = new CreateCategoryCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequest request)
        {
            var command = new UpdateCategoryCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            var command = new DeleteCategoryCommand(id);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
