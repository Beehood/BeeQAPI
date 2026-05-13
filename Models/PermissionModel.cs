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

        public string Name { get; set; }

        public string Code { get; set; }

        public string? Description { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class PermissionRequestDto
    {
        public long PermissionId { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string? Description { get; set; }

        public bool Status { get; set; }
    }
}
