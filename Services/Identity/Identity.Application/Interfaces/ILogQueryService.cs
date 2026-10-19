using Identity.Application.DTOs;
using LiteCommerce.Shared.Models;

namespace Identity.Application.Interfaces
{
    // Read side of the audit and activity logs, for the admin endpoints.
    public interface ILogQueryService
    {
        Task<BaseResponse<List<AuditLogResponse>>> GetAuditLogsAsync(GetAuditLogsQuery query, CancellationToken cancellationToken = default);

        Task<BaseResponse<AuditLogResponse>> GetAuditLogAsync(string id, CancellationToken cancellationToken = default);

        Task<BaseResponse<List<ActivityLogResponse>>> GetActivityLogsAsync(GetActivityLogsQuery query, CancellationToken cancellationToken = default);

        Task<BaseResponse<ActivityLogResponse>> GetActivityLogAsync(string id, CancellationToken cancellationToken = default);
    }
}
