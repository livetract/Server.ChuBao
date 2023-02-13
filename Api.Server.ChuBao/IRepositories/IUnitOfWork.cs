using Api.Server.ChuBao.Data;
using System;
using System.Threading.Tasks;

namespace Api.Server.ChuBao.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Contact> Contacts { get; }
        Task<int> CommitAsync();
    }
}
