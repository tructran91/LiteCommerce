﻿using System.Linq.Expressions;

namespace Catalog.Core.Repositories
{
    public interface IBaseRepository<T>
    {
        Task<List<T>> GetAllAsync(bool disableTracking = true, bool isDeletedIncluded = false);

        Task<IReadOnlyList<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
                                        Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
                                        string includeString = null,
                                        int pageNumber = 1, int pageSize = 10,
                                        bool disableTracking = true);

        IQueryable<T> Query();

        Task<int> CountAsync(Expression<Func<T, bool>> predicate = null);

        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate = null, bool isDeletedIncluded = false);

        Task<T> GetByIdAsync(Guid id);

        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);
    }
}
