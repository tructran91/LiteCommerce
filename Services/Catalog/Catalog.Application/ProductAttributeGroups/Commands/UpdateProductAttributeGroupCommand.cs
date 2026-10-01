using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductAttributeGroups.Commands
{
    public class UpdateProductAttributeGroupCommand : IRequest<BaseResponse<ProductAttributeGroupResponse>>
    {
        public string Id { get; set; }

        public UpdateProductAttributeGroupRequest Payload { get; set; }

        public UpdateProductAttributeGroupCommand(string id, UpdateProductAttributeGroupRequest payload)
        {
            Id = id;
            Payload = payload;
        }
    }
}
