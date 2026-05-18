using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class TokenModel
    {
        public long TokenId { get; set; }

        public long OrganizationId { get; set; }

        public long BranchId { get; set; }

        public string? BranchName { get; set; }

        public long BranchServiceId { get; set; }

        public string? ServiceName { get; set; }

        public long CustomerId { get; set; }

        public string? CustomerName { get; set; }

        public string? CustomerPhone { get; set; }

        public int TokenNumber { get; set; }

        public string? TokenDisplay { get; set; }

        public DateTime TokenDate { get; set; }

        public int Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class TokenRequestDto
    {
        public long TokenId { get; set; }

        public long OrganizationId { get; set; }

        public long BranchId { get; set; }

        public long BranchServiceId { get; set; }

        public long CustomerId { get; set; }

        public string? CustomerName { get; set; }

        public string? CustomerPhone { get; set; }

        public int Status { get; set; }

        public string? Action { get; set; } // LIST / GETBYID / STATUS / CALLNEXT

        public long CounterId { get; set; } // for counter operations
    }
}

