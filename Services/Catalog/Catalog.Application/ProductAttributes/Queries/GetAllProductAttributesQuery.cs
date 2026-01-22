using Catalog.Application.Responses;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductAttributes.Queries
{
    public class GetAllProductAttributesQuery : IRequest<BaseResponse<List<ProductAttributeResponse>>>
    {
        public int PageSize { get; set; } = PaginationSetting.DefaultPageSize;

        public int CurrentPage { get; set; } = PaginationSetting.DefaultCurrentPage;
    }
}
