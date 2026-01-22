using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductAttributes.Commands
{
    public class CreateProductAttributeCommand : IRequest<BaseResponse<ProductAttributeResponse>>
    {
        public CreateProductAttributeRequest Payload { get; set; }

        public CreateProductAttributeCommand(CreateProductAttributeRequest payload)
        {
            Payload = payload;
        }
    }
}
