using Data.Server.Chubao.Access;
using Data.Server.Chubao.Entities;
using Data.Server.ChuBao.Entities;
using System;
using System.Threading.Tasks;

namespace Data.Server.Chubao.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IGenericRepository<Contact> _contacts;
        public IGenericRepository<Contact> Contacts =>
            _contacts ?? new GenericRepository<Contact>(_context);
        public IGenericRepository<Record> Records { get; }

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

            Records = new GenericRepository<Record>(_context);
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
