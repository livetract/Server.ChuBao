using Data.Server.ChuBao.Entities;
using System;

namespace Data.Server.Chubao.Entities
{
    public class ContactRecord
    {
        public Guid ContactId  { get; set; }
        public Contact Contact { get; set; }
        public Guid RecordId { get; set; }
        public Record Record { get; set; }
    }
}
