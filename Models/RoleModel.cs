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

        public long? OrganizationId { get; set; }

        public string? OrganizationName { get; set; }


        public string? RoleName { get; set; }

        public string? RoleCode { get; set; }

        public string? Description { get; set; }

        public bool Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class RoleRequestDto
    {
        public long RoleId { get; set; }

        public long? OrganizationId { get; set; }

        public string? OrganizationName { get; set; }

        public string? RoleName { get; set; }

        public string? RoleCode { get; set; }

        public string? Description { get; set; }

        public bool? Status { get; set; }

        //public long? UserId_Login { get; set; }
    }
    public class RoleStatusRequestDto
    {
        public long RoleId { get; set; }
    }


}