using Catalog.Application.Behaviors;
using Catalog.Application.Requests;
using Catalog.Application.Responses;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductAttributeGroups.Commands
{
    public class UpdateProductAttributeGroupCommand : IRequest<BaseResponse<ProductAttributeGroupResponse>>, IActivityLoggable
    {
        public string Id { get; set; }

        public UpdateProductAttributeGroupRequest Payload { get; set; }

        public UpdateProductAttributeGroupCommand(string id, UpdateProductAttributeGroupRequest payload)
        {
            Id = id;
            Payload = payload;
        }

        ActivityAction IActivityLoggable.ActivityAction => ActivityAction.Update;

        string IActivityLoggable.ActivityEntityName => "ProductAttributeGroup";

        string? IActivityLoggable.ActivityEntityId => Id;

        string? IActivityLoggable.ActivityEntityDisplayName => Payload.Name;
    }
}
