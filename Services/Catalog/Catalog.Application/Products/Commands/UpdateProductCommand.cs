using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Products.Commands
{
    public class UpdateProductCommand : IRequest<BaseResponse<ProductResponse>>
    {
        public UpdateProductRequest Payload { get; set; }

        public UpdateProductCommand(UpdateProductRequest payload)
        {
            Payload = payload;
        }
    }
}
