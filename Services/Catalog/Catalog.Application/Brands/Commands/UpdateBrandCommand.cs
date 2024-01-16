using Catalog.Application.Requests;
using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Brands.Commands
{
    public class UpdateBrandCommand : IRequest<BaseResponse<BrandResponse>>
    {
        public UpdateBrandRequest Payload { get; set; }

        public UpdateBrandCommand(UpdateBrandRequest payload)
        {
            Payload = payload;
        }
    }
}
