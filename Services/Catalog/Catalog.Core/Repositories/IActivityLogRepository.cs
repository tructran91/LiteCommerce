using Catalog.Core.DTOs;
using Catalog.Core.Entities;

namespace Catalog.Core.Repositories
{
    public interface IActivityLogRepository
    {
        Task AddAsync(ActivityLog activityLog);

        Task<(List<ActivityLog> Items, int TotalCount)> GetPagedAsync(ActivityLogFilter filter, int currentPage, int pageSize);

        Task<ActivityLog?> GetByIdAsync(Guid id);
    }
}
