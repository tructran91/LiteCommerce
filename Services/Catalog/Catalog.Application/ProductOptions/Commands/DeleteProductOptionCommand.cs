using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductOptions.Commands
{
    public class DeleteProductOptionCommand : IRequest<BaseResponse<bool>>
    {
        public string Id { get; set; }

        public DeleteProductOptionCommand(string id)
        {
            Id = id;
        }
    }
}
