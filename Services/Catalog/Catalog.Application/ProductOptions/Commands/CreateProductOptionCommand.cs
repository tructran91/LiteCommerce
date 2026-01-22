using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductOptions.Commands
{
    public class CreateProductOptionCommand : IRequest<BaseResponse<ProductOptionResponse>>
    {
        public CreateProductOptionRequest Payload { get; set; }

        public CreateProductOptionCommand(CreateProductOptionRequest payload)
        {
            Payload = payload;
        }
    }
}
