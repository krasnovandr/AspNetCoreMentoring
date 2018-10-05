using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreMentoring.Infrastructure
{
    public class EfGenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly NorthwindContext _context;
        private readonly DbSet<TEntity> _dbSet;

        public EfGenericRepository(NorthwindContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindByAsync(Expression<Func<TEntity, bool>> match)
        {
            return await _dbSet.AsNoTracking().Where(match).ToListAsync();
        }
        public async Task<TEntity> FindByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task CreateAsync(TEntity item)
        {
            await _dbSet.AddAsync(item);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(TEntity item)
        {
            _dbSet.Remove(item);
            await _context.SaveChangesAsync();
        }



        public IEnumerable<TEntity> GetWithInclude(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return Include(includeProperties).ToList();
        }

        public async Task<IEnumerable<TEntity>> GetWithIncludeAsync(
            Expression<Func<TEntity, bool>> filter,
            int? page = 0,
            int? pageSize = null,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = Include(includeProperties);

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (page.HasValue && page != 0 && pageSize.HasValue)
            {
                query = query.Skip(((int)page - 1) * (int)pageSize);
            }

            if (pageSize.HasValue && pageSize != 0 )
            {
                query = query.Take((int)pageSize);
            }

            return await query.ToListAsync();
        }

        private IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _dbSet.AsNoTracking();
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }


    }
}
