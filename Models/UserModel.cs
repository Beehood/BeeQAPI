using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserModel
    {
        public long UserId { get; set; }

        public long OrganizationId { get; set; }

        public long BranchId { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public long RoleId { get; set; }   // from user_roles

        public int Status { get; set; }

        public DateTime CreatedAt { get; set; }
    }
    public class UserRequestDto
    {
        public long UserId { get; set; }

        public long OrganizationId { get; set; }

        public long BranchId { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        public long RoleId { get; set; }

        public int? Status { get; set; }

        public long? UserId_Login { get; set; }
    }
}