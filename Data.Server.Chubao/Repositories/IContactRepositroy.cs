using Core.Server.ChuBao.DTOs;
using Data.Server.ChuBao.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Server.Chubao.Repositories
{
    public interface IContactRepositroy : IGenericRepository<Contact>
    {
        Task<IEnumerable<ContactAMark>> GetContactWithMarksAsync();
    }
}
