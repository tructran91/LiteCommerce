using Catalog.Core.DTOs;
using Catalog.Core.Entities;

namespace Catalog.Core.Repositories
{
    // Read-only: audit rows are written by AuditLogInterceptor, not through the repository.
    public interface IAuditLogRepository
    {
        Task<(List<AuditLog> Items, int TotalCount)> GetPagedAsync(AuditLogFilter filter, int currentPage, int pageSize);

        Task<AuditLog?> GetByIdAsync(Guid id);
    }
}
