using Data.Server.Chubao.Entities;
using Data.Server.ChuBao.Entities;
using System;
using System.Threading.Tasks;

namespace Data.Server.Chubao.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IContactRepositroy Contacts { get; }
        IGenericRepository<Record> Records { get; }
        IGenericRepository<Mark> Marks { get; }
        Task<int> CommitAsync();
    }
}
