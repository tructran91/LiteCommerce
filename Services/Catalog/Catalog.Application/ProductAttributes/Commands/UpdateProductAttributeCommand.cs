using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductAttributes.Commands
{
    public class UpdateProductAttributeCommand : IRequest<BaseResponse<ProductAttributeResponse>>
    {
        public string Id { get; set; }

        public UpdateProductAttributeRequest Payload { get; set; }

        public UpdateProductAttributeCommand(string id, UpdateProductAttributeRequest payload)
        {
            Id = id;
            Payload = payload;
        }
    }
}
