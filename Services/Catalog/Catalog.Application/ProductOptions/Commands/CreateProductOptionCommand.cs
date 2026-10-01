using Catalog.Application.Behaviors;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductOptions.Commands
{
    public class CreateProductOptionCommand : IRequest<BaseResponse<ProductOptionResponse>>, IActivityLoggable
    {
        public CreateProductOptionRequest Payload { get; set; }

        public CreateProductOptionCommand(CreateProductOptionRequest payload)
        {
            Payload = payload;
        }

        ActivityAction IActivityLoggable.ActivityAction => ActivityAction.Create;

        string IActivityLoggable.ActivityEntityName => "ProductOption";

        string? IActivityLoggable.ActivityEntityId => null;

        string? IActivityLoggable.ActivityEntityDisplayName => Payload.Name;
    }
}
