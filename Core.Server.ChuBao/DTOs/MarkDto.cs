using System;

namespace Core.Server.ChuBao.DTOs
{
    public class MarkDto
    {
        public Guid Id { get; set; }
        public bool HasWeChat { get; set; }
        public bool HasArrived { get; set; }
        public bool HasDeposit { get; set; }
        public bool HasContract { get; set; }
    }
}
