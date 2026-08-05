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
            public long BranchServiceId { get; set; }
            public int TokenNumber { get; set; }
            public DateTime? TokenDate { get; set; }
            public string? TokenDisplay { get; set; }
            public string? CustomerName { get; set; }
            public string? CustomerPhone { get; set; }
            public int Status { get; set; }
            public int Priority { get; set; }
            public DateTime? CreatedAt { get; set; }

            // EXTRA DISPLAY FIELDS
            public string? OrganizationName { get; set; }
            public string? BranchName { get; set; }
            public string? ServiceName { get; set; }
    }

        public class TokenRequestDto
        {
            public long TokenId { get; set; }
            public long OrganizationId { get; set; }
            public long BranchId { get; set; }
            public long BranchServiceId { get; set; }
            public long CounterId { get; set; }

        public int TokenNumber { get; set; }
            public DateTime? TokenDate { get; set; }
            public string? TokenDisplay { get; set; }
            public string? CustomerName { get; set; }
            public string? CustomerPhone { get; set; }
            public int Status { get; set; }
            public int Priority { get; set; }

        }
    public class TokenStatusModel
    {
        public int StatusId { get; set; }
        public string? StatusName { get; set; }
    }
    public class TokenGenerateResponseDto
    {
        public int TokenId { get; set; }
        public string Token { get; set; } = "";
        public long BranchId { get; set; }
    }
}


