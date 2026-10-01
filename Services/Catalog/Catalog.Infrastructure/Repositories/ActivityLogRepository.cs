using Catalog.Core.DTOs;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories
{
    public class ActivityLogRepository : IActivityLogRepository
    {
        private readonly CatalogContext _dbContext;

        public ActivityLogRepository(CatalogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(ActivityLog activityLog)
        {
            _dbContext.ActivityLogs.Add(activityLog);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<(List<ActivityLog> Items, int TotalCount)> GetPagedAsync(ActivityLogFilter filter, int currentPage, int pageSize)
        {
            var query = _dbContext.ActivityLogs.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.EntityName))
                query = query.Where(x => x.EntityName == filter.EntityName);

            if (!string.IsNullOrWhiteSpace(filter.EntityId))
                query = query.Where(x => x.EntityId == filter.EntityId);

            if (filter.Action.HasValue)
                query = query.Where(x => x.Action == filter.Action);

            if (!string.IsNullOrWhiteSpace(filter.CorrelationId))
                query = query.Where(x => x.CorrelationId == filter.CorrelationId);

            if (!string.IsNullOrWhiteSpace(filter.Search))
                query = query.Where(x => x.Description.Contains(filter.Search) ||
                                         (x.EntityDisplayName != null && x.EntityDisplayName.Contains(filter.Search)));

            if (filter.FromDate.HasValue)
                query = query.Where(x => x.Timestamp >= filter.FromDate);

            if (filter.ToDate.HasValue)
                query = query.Where(x => x.Timestamp <= filter.ToDate);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(x => x.Timestamp)
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<ActivityLog?> GetByIdAsync(Guid id)
        {
            return await _dbContext.ActivityLogs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
