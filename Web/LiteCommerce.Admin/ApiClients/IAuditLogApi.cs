using LiteCommerce.Admin.Constants;
using LiteCommerce.Admin.Models.Business.AuditLog;
using LiteCommerce.Admin.Models.Common;
using Refit;

namespace LiteCommerce.Admin.ApiClients
{
    public interface IAuditLogApi
    {
        // Null filters are left out of the query string. Dates are UTC, sent in round-trip format so the Z survives.
        [Get(ApiRoutes.AuditLog.GetAll)]
        Task<BaseResponse<List<AuditLogResponse>>> GetAuditLogsAsync(
            int currentPage,
            int pageSize,
            string? entityName = null,
            string? entityId = null,
            string? action = null,
            string? correlationId = null,
            [Query(Format = "o")] DateTime? fromDate = null,
            [Query(Format = "o")] DateTime? toDate = null);

        [Get(ApiRoutes.AuditLog.GetById)]
        Task<BaseResponse<AuditLogResponse>> GetAuditLogAsync(string id);
    }
}
