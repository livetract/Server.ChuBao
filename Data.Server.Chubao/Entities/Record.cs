using System;

namespace Data.Server.Chubao.Entities
{
    public class Record
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public DateTime AddTime { get; set; }
        public string Booker { get; set; }

    }
}
