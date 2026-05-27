using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class AppointmentModel
    {
        public long AppointmentId { get; set; }
        public long OrganizationId { get; set; }
        public long BranchId { get; set; }
        public long ServiceId { get; set; }
        public string? ServiceName { get; set; }
        public long UserId { get; set; }

        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }

        public DateTime AppointmentDate { get; set; }
        public long TimeSlotId { get; set; }

        public int Status { get; set; }   // 1=Booked, 0=Cancelled, 2=Completed
        public long? TokenId { get; set; }
    }

    public class AppointmentRequestDto
    {
        public string Action { get; set; }   // LIST / GETBYID / INSERT / UPDATE / STATUS

        public long AppointmentId { get; set; }

        public long? OrganizationId { get; set; }
        public long? BranchId { get; set; }
        public long? ServiceId { get; set; }
        public long? UserId { get; set; }

        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }

        public DateTime? AppointmentDate { get; set; }
        public long? TimeSlotId { get; set; }

        public int? Status { get; set; }

        // LIST FILTER
        public string? SearchKey { get; set; }
        public int PageNo { get; set; }
    }

    public class AppointmentStatusRequestDto
    {
        public long AppointmentId { get; set; }
        public int Status { get; set; }
    }

}
