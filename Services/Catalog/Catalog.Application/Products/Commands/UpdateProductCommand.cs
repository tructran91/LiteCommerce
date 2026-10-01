using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Products.Commands
{
    public class UpdateProductCommand : IRequest<BaseResponse<ProductResponse>>
    {
        public string Id { get; set; }

        public UpdateProductRequest Payload { get; set; }

        public UpdateProductCommand(string id, UpdateProductRequest payload)
        {
            Id = id;
            Payload = payload;
        }
    }
}
