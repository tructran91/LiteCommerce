using Catalog.Application.Behaviors;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductAttributeGroups.Commands
{
    public class CreateProductAttributeGroupCommand : IRequest<BaseResponse<ProductAttributeGroupResponse>>, IActivityLoggable
    {
        public CreateProductAttributeGroupRequest Payload { get; set; }

        public CreateProductAttributeGroupCommand(CreateProductAttributeGroupRequest payload)
        {
            Payload = payload;
        }

        ActivityAction IActivityLoggable.ActivityAction => ActivityAction.Create;

        string IActivityLoggable.ActivityEntityName => "ProductAttributeGroup";

        string? IActivityLoggable.ActivityEntityId => null;

        string? IActivityLoggable.ActivityEntityDisplayName => Payload.Name;
    }
}
