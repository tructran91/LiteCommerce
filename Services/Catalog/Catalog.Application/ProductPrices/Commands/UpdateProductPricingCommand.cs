using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductPrices.Commands
{
    public class UpdateProductPricingCommand : IRequest<BaseResponse<List<ProductPricingResponse>>>
    {
        public List<UpdateProductPricingRequest> Payload { get; set; }

        public UpdateProductPricingCommand(List<UpdateProductPricingRequest> payload)
        {
            Payload = payload;
        }
    }
}
