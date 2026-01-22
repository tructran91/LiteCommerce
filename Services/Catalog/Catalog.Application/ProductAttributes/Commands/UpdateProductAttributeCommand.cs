using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductAttributes.Commands
{
    public class UpdateProductAttributeCommand : IRequest<BaseResponse<ProductAttributeResponse>>
    {
        public UpdateProductAttributeRequest Payload { get; set; }

        public UpdateProductAttributeCommand(UpdateProductAttributeRequest payload)
        {
            Payload = payload;
        }
    }
}
