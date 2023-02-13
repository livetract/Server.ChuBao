using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Server.ChuBao.IRepositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task Insert(T entity);
        Task InsertRange(IEnumerable<T> entities);
        Task Delete(object id);
        void DeleteRange(IEnumerable<T> entities);
        void Update(T entity);
        Task<IList<T>> GetAll(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            List<string> includes = null);
        Task<T> Get(Expression<Func<T, bool>> expression = null,
            List<string> includes = null);
    }
}
