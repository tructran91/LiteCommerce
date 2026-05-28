using Catalog.Application.Products.Commands;
using Catalog.Application.Products.Queries;
using Catalog.Application.Requests;
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

        [HttpPost("CreateProduct")]
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

        [HttpPut("UpdateProduct")]
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
    }
}
