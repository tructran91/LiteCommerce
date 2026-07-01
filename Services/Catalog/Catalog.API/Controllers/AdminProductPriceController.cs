using Catalog.Application.ProductPrices.Commands;
using Catalog.Application.ProductPrices.Queries;
using Catalog.Application.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
    [Route("api/admin/product-price")]
    [ApiController]
    public class AdminProductPriceController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdminProductPriceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductPricing([FromQuery] GetProductPricingQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProductPricing([FromBody] UpdateProductPricingListRequest request)
        {
            var command = new UpdateProductPricingCommand(request);
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
