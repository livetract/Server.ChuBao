using Api.Server.ChuBao.Data;
using Api.Server.ChuBao.IRepositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Api.Server.ChuBao.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        protected DbSet<T> Table { get; }

        public GenericRepository(
            AppDbContext context
            )
        {
            this._context = context;
            Table = context.Set<T>();
        }
        public async Task DeleteAsync(object id)
        {
            var entity = await Table.FindAsync(id);
            Table.Remove(entity);
        }

        public void Delete(T entity)
        {
            Table.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            Table.RemoveRange(entities);
        }

        public async Task<T> GetAsync(
            Expression<Func<T, bool>> expression = null, 
            List<string> includes = null)
        {
            IQueryable<T> query = Table;
            if (includes != null)
            {
                foreach (var item in includes)
                {
                    query = query.Include(item);
                }
            }
            return await query.AsNoTracking().FirstOrDefaultAsync(expression);
        }

        public async Task<IList<T>> GetAllAsync(
            Expression<Func<T, bool>> expression = null, 
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, 
            List<string> includes = null)
        {
            IQueryable<T> query = Table;

            if (expression != null)
            {
                query = query.Where(expression);
            }

            if (includes != null)
            {
                foreach (var property in includes)
                {
                    query = query.Include(property);
                }
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task InsertAsync(T entity)
        {
            await Table.AddAsync(entity);
        }

        public async Task InsertRangeAsync(IEnumerable<T> entities)
        {
            await Table.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            Table.Attach(entity);
            Table.Entry(entity).State = EntityState.Modified;
        }
    }
}
