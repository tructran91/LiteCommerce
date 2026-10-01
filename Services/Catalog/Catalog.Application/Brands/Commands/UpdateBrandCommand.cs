using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Brands.Commands
{
    public class UpdateBrandCommand : IRequest<BaseResponse<BrandResponse>>
    {
        public string Id { get; set; }

        public UpdateBrandRequest Payload { get; set; }

        public UpdateBrandCommand(string id, UpdateBrandRequest payload)
        {
            Id = id;
            Payload = payload;
        }
    }
}
