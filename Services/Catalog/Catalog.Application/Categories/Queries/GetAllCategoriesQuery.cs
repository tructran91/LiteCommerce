using Catalog.Application.Responses;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Brands.Queries
{
    public class GetAllCategoriesQuery : IRequest<BaseResponse<List<CategoryResponse>>>
    {
        public int PageSize { get; set; } = PaginationSetting.DefaultPageSize;

        public int CurrentPage { get; set; } = PaginationSetting.DefaultCurrentPage;
    }
}
