using Catalog.Application.Requests;
using Catalog.Application.Responses;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Brands.Commands
{
    public class CreateBrandCommand : IRequest<BaseResponse<BrandResponse>>
    {
        public CreateBrandRequest Payload { get; set; }

        public CreateBrandCommand(CreateBrandRequest payload)
        {
            Payload = payload;
        }
    }
}
