using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Brands.Queries
{
    public class GetBrandQuery : IRequest<BaseResponse<BrandResponse>>
    {
        public Guid Id { get; set; }

        public GetBrandQuery(Guid id)
        {
            Id = id;
        }
    }
}
