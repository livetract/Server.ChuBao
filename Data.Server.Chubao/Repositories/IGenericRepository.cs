using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Server.Chubao.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task InsertAsync(T entity);
        Task InsertRangeAsync(IEnumerable<T> entities);
        Task DeleteAsync(object id);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entities);
        void Update(T entity);
        Task<IList<T>> GetAllAsync(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<string> includes = null);
        Task<T> GetAsync(Expression<Func<T, bool>> expression = null,
            List<string> includes = null);
    }
}
