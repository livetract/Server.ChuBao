using System;

namespace Core.Server.ChuBao.DTOs
{
    public class MarkDto
    {
        public Guid Id { get; set; }
        public bool HasWeChat { get; set; } = false;
        public bool HasArrived { get; set; } = false;
        public bool HasDeposit { get; set; } = false;
        public bool HasContract { get; set; } = false;
        public bool IsLose { get; set; } = false;

        public Guid ContactId { get; set; }
    }
}
