using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class RolePermissionModel
    {
        public long Id { get; set; }

        public long RoleId { get; set; }

        public long PermissionId { get; set; }

        public string? PermissionName { get; set; }

        public string? PermissionCode { get; set; }
        public string? Module { get; set; }

        public bool IsAssigned { get; set; }
    }

    public class RolePermissionRequestDto
    {
        public long RoleId { get; set; }

        public long PermissionId { get; set; }  // for single insert/delete

        public string? PermissionIds { get; set; } //  for bulk insert (comma-separated)

        // Example: "1,2,3,4"
    }
}
