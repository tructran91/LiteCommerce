using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.ActivityLog;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IActivityLogApi
    {
        // Null filters are left out of the query string. Dates are UTC, sent in round-trip format so the Z survives.
        [Get(ApiRoutes.ActivityLog.GetAll)]
        Task<BaseResponse<List<ActivityLogResponse>>> GetActivityLogsAsync(
            int currentPage,
            int pageSize,
            string? search = null,
            string? entityName = null,
            string? action = null,
            [Query(Format = "o")] DateTime? fromDate = null,
            [Query(Format = "o")] DateTime? toDate = null);

        [Get(ApiRoutes.ActivityLog.GetById)]
        Task<BaseResponse<ActivityLogResponse>> GetActivityLogAsync(string id);
    }
}
