using Catalog.Application.Responses;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.Products.Queries
{
    public class GetAllProductsQuery : IRequest<BaseResponse<List<BasicProductResponse>>>
    {
        public int PageSize { get; set; } = PaginationSetting.DefaultPageSize;

        public int CurrentPage { get; set; } = PaginationSetting.DefaultCurrentPage;

        public string? Name { get; set; }

        public bool? IsFeatured { get; set; }

        public bool? IsAllowToOrder { get; set; }

        public bool? IsCallForPricing { get; set; }

        public bool? IsPublished { get; set; }

        public string? SortBy { get; set; }

        public string? SortDirection { get; set; }
    }
}
