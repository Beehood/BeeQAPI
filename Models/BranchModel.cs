using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BranchModel
    {
        public long BranchId { get; set; }
        public long OrganizationId { get; set; }
        public string? OrganizationName { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Timezone { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class BranchRequestDto
    {
        public long BranchId { get; set; }
        public long OrganizationId { get; set; }
        public string BranchName { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Timezone { get; set; }
        public bool? Status { get; set; }
        public long? UserId { get; set; }
    }
    public class BranchStatusRequestDto
    {
        public long BranchId { get; set; }
    }

}
