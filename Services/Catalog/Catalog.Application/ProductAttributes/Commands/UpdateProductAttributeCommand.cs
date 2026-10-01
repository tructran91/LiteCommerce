using Catalog.Application.Behaviors;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductAttributes.Commands
{
    public class UpdateProductAttributeCommand : IRequest<BaseResponse<ProductAttributeResponse>>, IActivityLoggable
    {
        public string Id { get; set; }

        public UpdateProductAttributeRequest Payload { get; set; }

        public UpdateProductAttributeCommand(string id, UpdateProductAttributeRequest payload)
        {
            Id = id;
            Payload = payload;
        }

        ActivityAction IActivityLoggable.ActivityAction => ActivityAction.Update;

        string IActivityLoggable.ActivityEntityName => "ProductAttribute";

        string? IActivityLoggable.ActivityEntityId => Id;

        string? IActivityLoggable.ActivityEntityDisplayName => Payload.Name;
    }
}
