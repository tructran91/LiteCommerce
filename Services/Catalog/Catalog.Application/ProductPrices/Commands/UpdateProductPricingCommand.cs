using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductPrices.Commands
{
    public class UpdateProductPricingCommand : IRequest<BaseResponse<List<ProductPricingResponse>>>
    {
        public UpdateProductPricingListRequest Payload { get; set; }

        public UpdateProductPricingCommand(UpdateProductPricingListRequest payload)
        {
            Payload = payload;
        }
    }
}
