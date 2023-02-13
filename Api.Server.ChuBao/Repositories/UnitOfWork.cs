using Api.Server.ChuBao.Data;
using Api.Server.ChuBao.IRepositories;
using System;
using System.Threading.Tasks;

namespace Api.Server.ChuBao.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IGenericRepository<Contact> _contacts;
        public IGenericRepository<Contact> Contacts => 
            _contacts ?? new GenericRepository<Contact>(_context);

        public UnitOfWork(AppDbContext context)
        {
            this._context = context;
        }

        public Task<int> CommitAsync(AppDbContext context)
        {
            throw new System.NotImplementedException();
        }

        public async Task<int> CommitAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
