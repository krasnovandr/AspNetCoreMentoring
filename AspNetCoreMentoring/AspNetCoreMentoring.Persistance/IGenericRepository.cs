using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AspNetCoreMentoring.Infrastructure
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> FindByAsync(Expression<Func<T, bool>> predicate);
        Task<T> FindByIdAsync(int id);
        Task<T> CreateAsync(T entity);
        Task DeleteAsync(T entity);
        Task UpdateAsync(T entity);

        Task<IEnumerable<T>> GetWithIncludeAsync(
            Expression<Func<T, bool>> filter,
            int? page = 0,
            int? pageSize = null,
            params Expression<Func<T, object>>[] includeProperties);
    }
}