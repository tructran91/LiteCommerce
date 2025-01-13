using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Brands.Commands
{
    public class DeleteBrandCommand : IRequest<BaseResponse<BrandResponse>>
    {
        public string Id { get; set; }

        public DeleteBrandCommand(string id)
        {
            Id = id;
        }
    }
}
