using System;

namespace Core.Server.ChuBao.DTOs
{
    public class ContactAMark
    {
        public Guid ContactId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Complex { get; set; }
        public string Door { get; set; }
        public bool HasWeChat { get; set; } = false;
        public bool HasArrived { get; set; } = false;
        public bool HasDeposit { get; set; } = false;
        public bool HasContract { get; set; } = false;
        public bool IsLose { get; set; } = false;
        public bool IsAttention { get; set; } = false;
    }
}
