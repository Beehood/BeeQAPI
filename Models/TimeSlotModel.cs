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

        // Organization
        public long OrganizationId { get; set; }
        public string? OrganizationName { get; set; }

        // Branch
        public long BranchId { get; set; }
        public string? BranchName { get; set; }

        // Service
        public long ServiceId { get; set; }
        public string? ServiceName { get; set; }

        // Time Slot
        public int DayOfWeek { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public int MaxCapacity { get; set; }

        public int Status { get; set; }
    }


    public class TimeSlotRequestDto
    {
        public string? Action { get; set; }

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
