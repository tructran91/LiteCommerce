using Catalog.Application.ProductPrices.Commands;
using Catalog.Application.ProductPrices.Queries;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/admin/product-price")]
    [ApiController]
    public class AdminProductPriceController : BaseApiController
    {
        private readonly IMediator _mediator;

        public AdminProductPriceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<List<ProductPricingResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProductPricing([FromQuery] GetProductPricingQuery query)
        {
            var result = await _mediator.Send(query);
            return ToActionResult(result);
        }

        // PATCH, not PUT: only the listed products' prices change; the rest of the collection is untouched.
        [HttpPatch]
        [ProducesResponseType(typeof(BaseResponse<List<ProductPricingResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<List<ProductPricingResponse>>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateProductPricing([FromBody] UpdateProductPricingListRequest request)
        {
            var command = new UpdateProductPricingCommand(request);
            var result = await _mediator.Send(command);
            return ToActionResult(result);
        }
    }
}
