using System;

namespace Core.Server.ChuBao.DTOs
{
    public class CreateRecordDto
    {
        public string Content { get; set; }
        public string Booker { get; set; }
        public Guid ContactId { get; set; }
    }
}
