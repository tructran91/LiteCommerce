using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.ActivityLog;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    // Points at the Identity service. Same route as Catalog's activity log, different filters (no entity, has result).
    public interface IIdentityActivityLogApi
    {
        // Null filters are left out of the query string. Dates are UTC, sent in round-trip format so the Z survives.
        [Get(ApiRoutes.ActivityLog.GetAll)]
        Task<BaseResponse<List<ActivityLogResponse>>> GetActivityLogsAsync(
            int currentPage,
            int pageSize,
            string? search = null,
            string? action = null,
            bool? isSuccess = null,
            string? correlationId = null,
            [Query(Format = "o")] DateTime? fromDate = null,
            [Query(Format = "o")] DateTime? toDate = null);

        [Get(ApiRoutes.ActivityLog.GetById)]
        Task<BaseResponse<ActivityLogResponse>> GetActivityLogAsync(string id);
    }
}
