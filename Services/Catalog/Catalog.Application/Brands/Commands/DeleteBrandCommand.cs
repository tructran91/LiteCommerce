using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Brands.Commands
{
    public class DeleteBrandCommand : IRequest<BaseResponse<bool>>
    {
        public string Id { get; set; }

        public DeleteBrandCommand(string id)
        {
            Id = id;
        }
    }
}
