using Catalog.Application.Responses;
using Catalog.Core.Enums;
using LiteCommerce.Shared.Constants;
using LiteCommerce.Shared.Models;
using MediatR;

namespace Catalog.Application.ActivityLogs.Queries
{
    public class GetAllActivityLogsQuery : IRequest<BaseResponse<List<ActivityLogResponse>>>
    {
        public int PageSize { get; set; } = PaginationSetting.DefaultPageSize;

        public int CurrentPage { get; set; } = PaginationSetting.DefaultCurrentPage;

        public string? EntityName { get; set; }

        public string? EntityId { get; set; }

        public ActivityAction? Action { get; set; }

        public string? CorrelationId { get; set; }

        public string? Search { get; set; }

        // UTC, inclusive.
        public DateTime? FromDate { get; set; }

        // UTC, inclusive.
        public DateTime? ToDate { get; set; }
    }
}
