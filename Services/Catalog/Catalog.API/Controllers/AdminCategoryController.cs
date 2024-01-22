using Catalog.Application.Brands.Queries;
using Catalog.Application.Categories.Commands;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Net;

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
        [ProducesResponseType(typeof(BaseResponse<List<CategoryResponse>>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetAllCategories()
        {
            var query = new GetAllCategoriesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("GetCategoryById/{id}")]
        [ProducesResponseType(typeof(BaseResponse<CategoryResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetCategoryById(string id)
        {
            var query = new GetCategoryQuery(Guid.Parse(id));
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPost("CreateCategory")]
        [ProducesResponseType(typeof(BaseResponse<CategoryResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateCategory([FromBody] CreateCategoryRequest request)
        {
            var command = new CreateCategoryCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut]
        [Route("UpdateCategory")]
        [ProducesResponseType(typeof(BaseResponse<CategoryResponse>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequest request)
        {
            var command = new UpdateCategoryCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
