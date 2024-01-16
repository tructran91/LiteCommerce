using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Brands.Queries
{
    public class GetAllBrandsQuery : IRequest<BaseResponse<List<BrandResponse>>>
    {
    }
}
