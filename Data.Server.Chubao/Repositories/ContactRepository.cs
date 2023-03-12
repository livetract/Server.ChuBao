using Core.Server.ChuBao.DTOs;
using Data.Server.Chubao.Access;
using Data.Server.ChuBao.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Data.Server.Chubao.Repositories
{
    public class ContactRepository : GenericRepository<Contact>, IContactRepositroy
    {
        private readonly AppDbContext _context;

        public ContactRepository(AppDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<ContactAMark>> GetContactWithMarksAsync()
        {
            var query = from c in _context.Contacts
                        from m in _context.Marks.Where(m => m.ContactId == c.Id)
                        select new ContactAMark { ContactId = m.ContactId, Name = c.Name, Complex = c.Complex, Door = c.Door, Phone = c.Phone,
                            HasWeChat = m.HasWeChat, HasArrived = m.HasArrived, HasDeposit = m.HasDeposit, HasContract = m.HasContract, IsAttention = m.IsAttention, IsLose = m.IsLose };
                        //select new {c.Name, c.Phone, c.Complex, c.Door, m.ContactId, m.HasWeChat,m.HasContract, m.HasDeposit, m.HasArrived, m.IsAttention, m.IsLose};
            var result = await query.AsNoTracking().ToListAsync();
            var cm = result;
           
            return cm;
            //throw new NotImplementedException();
        }
    }
}
