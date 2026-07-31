using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class PermissionModel
    {
        public long PermissionId { get; set; }

        public string PermissionName { get; set; } = string.Empty;

        public string? PermissionCode { get; set; }

        public string? Module { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedAt { get; set; }
        public string PermissionScope { get; set; } = string.Empty;
    }

    public class PermissionRequestDto
    {
        public long PermissionId { get; set; }

        public string PermissionName { get; set; } = string.Empty;

        public string? PermissionCode { get; set; }

        public string? Module { get; set; }

        public bool? Status { get; set; }

        public string? SearchKey { get; set; }

        public int? PageNo { get; set; }

        public int? PageSize { get; set; }

        public long? UserId { get; set; }
        public string PermissionScope { get; set; } = string.Empty;
    }
    public class PermissionStatusRequestDto
    {
        public long PermissionId { get; set; }
    }
}
