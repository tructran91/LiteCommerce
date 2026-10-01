using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductOptions.Commands
{
    public class UpdateProductOptionCommand : IRequest<BaseResponse<ProductOptionResponse>>
    {
        public string Id { get; set; }

        public UpdateProductOptionRequest Payload { get; set; }

        public UpdateProductOptionCommand(string id, UpdateProductOptionRequest payload)
        {
            Id = id;
            Payload = payload;
        }
    }
}
