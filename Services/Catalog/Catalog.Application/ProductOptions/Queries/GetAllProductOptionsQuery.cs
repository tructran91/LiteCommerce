using Catalog.Application.Responses;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductOptions.Queries
{
    public class GetAllProductOptionsQuery : IRequest<BaseResponse<List<ProductOptionResponse>>>
    {
        public int PageSize { get; set; } = PaginationSetting.DefaultPageSize;

        public int CurrentPage { get; set; } = PaginationSetting.DefaultCurrentPage;
    }
}
