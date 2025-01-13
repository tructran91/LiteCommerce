using Catalog.Core.Entities;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using LiteCommerce.Shared.Constants;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Catalog.Infrastructure.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly CatalogContext _dbContext;
        private DbSet<T> _dbSet;

        public BaseRepository(CatalogContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<List<T>> GetAllAsync(bool disableTracking = true, bool isDeletedIncluded = false)
        {
            IQueryable<T> query = _dbSet;
            if (disableTracking) query = query.AsNoTracking();
            if (!isDeletedIncluded) query = query.Where(t => !t.IsDeleted);

            return await query.ToListAsync();
        }

        public async Task<IReadOnlyList<T>> GetAsync(
            Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includeString = null,
            int pageNumber = PaginationSetting.DefaultCurrentPage,
            int pageSize = PaginationSetting.DefaultPageSize,
            bool disableTracking = true)
        {
            IQueryable<T> query = _dbSet;

            if (disableTracking)
                query = query.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(includeString))
                query = query.Include(includeString);

            if (predicate != null)
                query = query.Where(predicate);

            query = orderBy != null ? orderBy(query) : query.OrderBy(x => x.CreatedDate);

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return await query.ToListAsync();
        }

        public IQueryable<T> Query()
        {
            return _dbSet;
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            IQueryable<T> query = _dbSet;
            query = query.AsNoTracking();

            if (predicate != null) query = query.Where(predicate);

            return await query.CountAsync();
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate = null, bool isDeletedIncluded = false)
        {
            IQueryable<T> query = _dbSet;
            query = query.AsNoTracking();

            if (predicate != null) query = query.Where(predicate);
            if (!isDeletedIncluded) query = query.Where(t => !t.IsDeleted);

            return await query.AnyAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbSet.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
        }

        public async Task<T> AddAsync(T entity)
        {
            _dbSet.Add(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
        }
    }
}
