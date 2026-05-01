using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RoleModel
    {
        public long RoleId { get; set; }
        public long OrganizationId { get; set; }

        public string RoleName { get; set; } = string.Empty;
        public string? RoleCode { get; set; }
        public string? Description { get; set; }

        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class RoleRequestDto
    {
        public long RoleId { get; set; }
        public long OrganizationId { get; set; }

        public string RoleName { get; set; } = string.Empty;
        public string? RoleCode { get; set; }
        public string? Description { get; set; }

        public int? Status { get; set; }

        public string? SearchKey { get; set; }
        public int? PageNo { get; set; }
        public int? PageSize { get; set; }

        public long? UserId { get; set; }
    }
}
