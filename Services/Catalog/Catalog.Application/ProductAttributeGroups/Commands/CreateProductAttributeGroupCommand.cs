using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductAttributeGroups.Commands
{
    public class CreateProductAttributeGroupCommand : IRequest<BaseResponse<ProductAttributeGroupResponse>>
    {
        public CreateProductAttributeGroupRequest Payload { get; set; }

        public CreateProductAttributeGroupCommand(CreateProductAttributeGroupRequest payload)
        {
            Payload = payload;
        }
    }
}
