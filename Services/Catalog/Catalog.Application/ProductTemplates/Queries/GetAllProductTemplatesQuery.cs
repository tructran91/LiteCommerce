using Catalog.Application.Responses;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ProductTemplates.Queries
{
    public class GetAllProductTemplatesQuery : IRequest<BaseResponse<List<ProductTemplateResponse>>>
    {
        public int PageSize { get; set; } = PaginationSetting.DefaultPageSize;

        public int CurrentPage { get; set; } = PaginationSetting.DefaultCurrentPage;
    }
}
