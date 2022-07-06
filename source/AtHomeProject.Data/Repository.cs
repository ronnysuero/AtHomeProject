using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AtHomeProject.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AtHomeProject.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _db;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(ApplicationDbContext db)
        {
            _db = db;
            _dbSet = db.Set<TEntity>();
        }

        private static async Task<IEnumerable<TEntity>> GetAsync(
            int page,
            int pageSize,
            IQueryable<TEntity> query,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
        )
        {
            var skip = (page - 1) * pageSize;

            var result = orderBy != null
                ? await orderBy(query).Skip(skip).Take(pageSize).AsNoTracking().ToListAsync()
                : await query.Skip(skip).Take(pageSize).AsNoTracking().ToListAsync();

            return result;
        }

        public Task<int> CountAsync => _dbSet.CountAsync();

        public async Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = ""
        )
        {
            var query = _dbSet.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            query = includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return orderBy != null ? await orderBy(query).ToListAsync() : await query.ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAsync(
            int page,
            int pageSize,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
        ) => await GetAsync(page, pageSize, _dbSet.AsQueryable(), orderBy);

        public async Task<(IEnumerable<TEntity> Results, int RowCount)> GetAsync(
            int page,
            int pageSize,
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null
        )
        {
            var query = _dbSet.AsQueryable();

            if (filter != null)
                query = query.Where(filter);

            var count = query.Count();

            return (await GetAsync(page, pageSize, query, orderBy), count);
        }

        public async Task<TEntity> GetByKeyAsync(object key) => await _dbSet.FindAsync(key);

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter) =>
            await _dbSet.Where(filter).FirstOrDefaultAsync();

        public async Task InsertAsync(TEntity entity) => await _dbSet.AddAsync(entity);

        public async Task InsertRangeAsync(params TEntity[] entities) => await _dbSet.AddRangeAsync(entities);

        public async Task DeleteAsync(params object[] keys)
        {
            var entityToDelete = await _dbSet.FindAsync(keys);
            Delete(entityToDelete);
        }

        public void Delete(TEntity entityToDelete)
        {
            if (_db.Entry(entityToDelete).State == EntityState.Detached)
                _dbSet.Attach(entityToDelete);

            _dbSet.Remove(entityToDelete);
        }

        public void Update(TEntity entityToUpdate)
        {
            _dbSet.Attach(entityToUpdate);
            _db.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
