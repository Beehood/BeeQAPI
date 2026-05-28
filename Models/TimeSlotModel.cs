using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TimeSlotModel
    {
        public long SlotId { get; set; }

        public long BranchId { get; set; }

        public long ServiceId { get; set; }

        public string? ServiceName { get; set; }   // from JOIN

        public int DayOfWeek { get; set; }         // 1 = Monday

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public int MaxCapacity { get; set; }

        public int Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    public class TimeSlotRequestDto
    {
        public string? Action { get; set; }   // LIST / INSERT / UPDATE / STATUS / DROPDOWN

        public long SlotId { get; set; }

        public long BranchId { get; set; }

        public long ServiceId { get; set; }

        public int DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public int MaxCapacity { get; set; }

        public int Status { get; set; }

        public string? SearchKey { get; set; }

        public int? PageNo { get; set; }

    }
    public class TimeSlotStatusRequestDto
    {
        public long SlotId { get; set; }
    }

}
