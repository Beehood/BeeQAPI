using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class QueueModel
    {
        public long TokenId { get; set; }

        public long OrganizationId { get; set; }
        public long BranchId { get; set; }
        public long BranchServiceId { get; set; }

        public int TokenNumber { get; set; }
        public string TokenDisplay { get; set; }

        public string? CustomerName { get; set; }
        public string? CustomerPhone { get; set; }

        public int Status { get; set; }   // 1=Waiting, 2=Called, 3=Completed, 4=Cancelled, 5=Transferred
        public int Priority { get; set; }

        //public long? CounterId { get; set; }   // IMPORTANT for queue monitor

        public DateTime CreatedAt { get; set; }
    }

public class QueueRequestDto
{
    public string Action { get; set; }   // CALL_NEXT, COMPLETE, RECALL, TRANSFER

    public long TokenId { get; set; }
    public long CounterId { get; set; }

    public long BranchId { get; set; }
    public long BranchServiceId { get; set; }

    public string? CustomerName { get; set; }
    public string? CustomerPhone { get; set; }

    public int Priority { get; set; }
}
    public class QueueDisplayModel
    {
        public int TokenNumber { get; set; }
        public string TokenDisplay { get; set; }
        //public long? CounterId { get; set; }

        public int Status { get; set; }
    }
    public class QueueDisplayRequest
    {
        public long BranchId { get; set; }
    }
} 