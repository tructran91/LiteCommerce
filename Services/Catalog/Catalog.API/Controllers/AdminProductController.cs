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
        [ProducesResponseType(typeof(BaseResponse<List<BasicProductResponse>>), 200)]
        public async Task<IActionResult> GetAllProducts([FromQuery] GetAllProductsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(BaseResponse<ProductResponse>), 200)]
        public async Task<ActionResult> GetProductById(string id)
        {
            var query = new GetProductQuery(id);
            var result = await _mediator.Send(query);

            if (result.Data != null && result.IsSuccess)
            {
                result.Data.ThumbnailImageUrl = BuildImageUrl(result.Data.ThumbnailImageUrl);

                foreach (var image in result.Data.ProductImages)
                {
                    image.MediaUrl = BuildImageUrl(image.MediaUrl);
                }

                foreach (var document in result.Data.ProductDocuments)
                {
                    document.MediaUrl = BuildImageUrl(document.MediaUrl);
                }
            }

            return Ok(result);
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<ProductResponse>), 200)]
        public async Task<ActionResult> CreateProduct([FromForm] CreateProductRequest request)
        {
            var command = new CreateProductCommand(request);
            var result = await _mediator.Send(command);

            if (result.Data != null && result.IsSuccess)
            {
                result.Data.ThumbnailImageUrl = BuildImageUrl(result.Data.ThumbnailImageUrl);

                foreach (var image in result.Data.ProductImages)
                {
                    image.MediaUrl = BuildImageUrl(image.MediaUrl);
                }

                foreach (var document in result.Data.ProductDocuments)
                {
                    document.MediaUrl = BuildImageUrl(document.MediaUrl);
                }
            }

            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(BaseResponse<ProductResponse>), 200)]
        public async Task<ActionResult> UpdateProduct([FromForm] UpdateProductRequest request)
        {
            var command = new UpdateProductCommand(request);
            var result = await _mediator.Send(command);

            if (result.Data != null && result.IsSuccess)
            {
                result.Data.ThumbnailImageUrl = BuildImageUrl(result.Data.ThumbnailImageUrl);

                foreach (var image in result.Data.ProductImages)
                {
                    image.MediaUrl = BuildImageUrl(image.MediaUrl);
                }

                foreach (var document in result.Data.ProductDocuments)
                {
                    document.MediaUrl = BuildImageUrl(document.MediaUrl);
                }
            }

            return Ok(result);
        }

        [HttpPost("upload-content-image")]
        [ProducesResponseType(typeof(BaseResponse<string>), 200)]
        [ProducesResponseType(typeof(BaseResponse<object>), 400)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> UploadContentImage([FromForm] UploadContentImageRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("No file provided");

            var command = new UploadContentImageCommand(request);
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
            {
                var fullUrl = BuildImageUrl(result.Data);
                return Ok(new { url = fullUrl });
            }

            return Ok(result);
        }
    }
}
