using Catalog.Core.DTOs;
using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly CatalogContext _dbContext;

        public AuditLogRepository(CatalogContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<(List<AuditLog> Items, int TotalCount)> GetPagedAsync(AuditLogFilter filter, int currentPage, int pageSize)
        {
            var query = _dbContext.AuditLogs.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.EntityName))
                query = query.Where(x => x.EntityName == filter.EntityName);

            if (!string.IsNullOrWhiteSpace(filter.EntityId))
                query = query.Where(x => x.EntityId == filter.EntityId);

            if (filter.Action.HasValue)
                query = query.Where(x => x.Action == filter.Action);

            if (!string.IsNullOrWhiteSpace(filter.CorrelationId))
                query = query.Where(x => x.CorrelationId == filter.CorrelationId);

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

        public async Task<AuditLog?> GetByIdAsync(Guid id)
        {
            return await _dbContext.AuditLogs.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
