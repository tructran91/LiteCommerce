using Catalog.Application.Behaviors;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductPrices.Commands
{
    public class UpdateProductPricingCommand : IRequest<BaseResponse<List<ProductPricingResponse>>>, IActivityLoggable
    {
        public UpdateProductPricingListRequest Payload { get; set; }

        public UpdateProductPricingCommand(UpdateProductPricingListRequest payload)
        {
            Payload = payload;
        }

        ActivityAction IActivityLoggable.ActivityAction => ActivityAction.BulkUpdate;

        string IActivityLoggable.ActivityEntityName => "ProductPrice";

        string? IActivityLoggable.ActivityEntityId => null;

        string? IActivityLoggable.ActivityEntityDisplayName => $"{Payload.Items.Count} product(s)";
    }
}
