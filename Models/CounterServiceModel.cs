using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CounterServiceModel
    {
        public string? OrganizationName { get; set; }

        public string? BranchName { get; set; }

        public long CounterServiceId { get; set; }

        public long CounterId { get; set; }

        public long BranchServiceId { get; set; }

        // Display
        public string? CounterName { get; set; }

        public string? ServiceName { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class CounterServiceRequestDto
    {
        public long CounterServiceId { get; set; }

        public long CounterId { get; set; }

        public long BranchServiceId { get; set; }

        public bool? Status { get; set; }

        public long? UserId { get; set; }
    }
    public class CounterServiceStatusRequestDto
    {
        public long CounterServiceId { get; set; }
    }
}
