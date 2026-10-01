using Catalog.Application.Products.Commands;
using Catalog.Application.Products.Queries;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/admin/product")]
    [ApiController]
    public class AdminProductController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdminProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<BasicProductResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductsQuery query)
        {
            var result = await _mediator.Send(query);
            return ToActionResult(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<ProductResponse>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetProductById(string id)
        {
            var query = new GetProductQuery(id);
            var result = await _mediator.Send(query);
            BuildMediaUrls(result);
            return ToActionResult(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<ProductResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<ProductResponse>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequest request)
        {
            var command = new CreateProductCommand(request);
            var result = await _mediator.Send(command);
            BuildMediaUrls(result);
            return ToCreatedResult(result, nameof(GetProductById), product => new { id = product.Id });
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ProductResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<ProductResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<ProductResponse>), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateProduct(string id, [FromForm] UpdateProductRequest request)
        {
            var command = new UpdateProductCommand(id, request);
            var result = await _mediator.Send(command);
            BuildMediaUrls(result);
            return ToActionResult(result);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<bool>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var command = new DeleteProductCommand(id);
            var result = await _mediator.Send(command);
            return ToActionResult(result);
        }

        [HttpPost("upload-content-image")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadContentImage([FromForm] UploadContentImageRequest request)
        {
            var command = new UploadContentImageCommand(request);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return ToActionResult(result);

            result.Data = BuildImageUrl(result.Data)!;
            return Created(result.Data, result);
        }

        private void BuildMediaUrls(BaseResponse<ProductResponse> result)
        {
            if (result.Data == null || !result.IsSuccess)
                return;

            result.Data.ThumbnailImageUrl = BuildImageUrl(result.Data.ThumbnailImageUrl);

            foreach (var media in result.Data.ProductImages.Concat(result.Data.ProductDocuments))
            {
                media.MediaUrl = BuildImageUrl(media.MediaUrl);
            }
        }
    }
}
