using Catalog.Application.Responses;
using LiteCommerce.Shared.Constants;
using MediatR;

namespace Catalog.Application.Brands.Queries
{
    public class GetAllBrandsQuery : IRequest<BaseResponse<List<BrandResponse>>>
    {
        public int PageSize { get; set; } = Pagination.DefaultPageSize;

        public int CurrentPage { get; set; } = Pagination.DefaultCurrentPage;
    }
}
