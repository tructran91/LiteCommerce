using Catalog.Application.Requests;
using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Products.Commands
{
    public class CreateProductCommand : IRequest<BaseResponse<ProductResponse>>
    {
        public CreateProductRequest Payload { get; set; }

        public CreateProductCommand(CreateProductRequest payload)
        {
            Payload = payload;
        }
    }
}
