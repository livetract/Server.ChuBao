using System;

namespace Data.Server.Chubao.Entities
{
    public class Mark
    {
        public Guid Id { get; set; }
        public bool HasWeChat { get; set; }
        public bool HasArrived { get; set; }
        public bool HasDeposit { get; set; }
        public bool HasContract { get; set; }
        public bool IsLose { get; set; }

        public Guid ContactId { get; set; }
    }
}
