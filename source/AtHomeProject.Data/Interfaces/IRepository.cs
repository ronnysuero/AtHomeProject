using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AtHomeProject.Data.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<int> CountAsync { get; }

        Task<IEnumerable<TEntity>> GetAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = ""
        );
        
        Task<TEntity> GetByKeyAsync(object key);

        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> filter);

        Task InsertAsync(TEntity entity);

        Task InsertRangeAsync(params TEntity[] entities);

        Task DeleteAsync(params object[] keys);

        void Delete(TEntity entityToDelete);
    }
}
