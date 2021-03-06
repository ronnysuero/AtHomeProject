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
    }
}
