using Catalog.Application.Behaviors;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductAttributes.Commands
{
    public class CreateProductAttributeCommand : IRequest<BaseResponse<ProductAttributeResponse>>, IActivityLoggable
    {
        public CreateProductAttributeRequest Payload { get; set; }

        public CreateProductAttributeCommand(CreateProductAttributeRequest payload)
        {
            Payload = payload;
        }

        ActivityAction IActivityLoggable.ActivityAction => ActivityAction.Create;

        string IActivityLoggable.ActivityEntityName => "ProductAttribute";

        string? IActivityLoggable.ActivityEntityId => null;

        string? IActivityLoggable.ActivityEntityDisplayName => Payload.Name;
    }
}
