using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductAttributes.Commands
{
    public class DeleteProductAttributeCommand : IRequest<BaseResponse<bool>>
    {
        public string Id { get; set; }

        public DeleteProductAttributeCommand(string id)
        {
            Id = id;
        }
    }
}
