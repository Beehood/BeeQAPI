using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BranchServiceModel
    {
        public long OrganizationId { get; set; }
        public long BranchServiceId { get; set; }
        public string? OrganizationName { get; set; }
        public long BranchId { get; set; }
        public long ServiceId { get; set; }

        // Dropdown display values
        public string? BranchName { get; set; }
        public string? ServiceName { get; set; }

        public string Prefix { get; set; } = string.Empty;
        public int DailyLimit { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class BranchServiceRequestDto
    {
        public long OrganizationId { get; set; }
        public long BranchServiceId { get; set; }
        public long BranchId { get; set; }
        public long ServiceId { get; set; }

        public string Prefix { get; set; } = string.Empty;
        public int DailyLimit { get; set; }
        public bool? Status { get; set; }
        public long? UserId { get; set; }
    }
    public class BranchServiceStatusRequestDto
    {
        public long BranchServiceId { get; set; }
    }

}
