using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CounterModel
    {
        public long CounterId { get; set; }
        public long OrganizationId { get; set; }

        public string? OrganizationName { get; set; }

        // Reference to Branch
        public long BranchId { get; set; }
     

        public string CounterName { get; set; }

        public string? CounterNumber { get; set; }
        public string? BranchName { get; set; }


        // 1 = Active, 0 = Inactive
        public bool Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class CounterRequestDto
    {
        public long CounterId { get; set; }

        // Required for create/update
        public long BranchId { get; set; }

        public string CounterName { get; set; }
        public string? CounterNumber { get; set; }

        public string? CounterCode { get; set; }


        public bool Status { get; set; }

        // Optional if using JWT, otherwise keep
        // public long UserId { get; set; }
    }
    public class CounterStatusRequestDto
    {
        public long CounterId { get; set; }
    }
}
