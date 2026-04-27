using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class UserProfileDetails
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? ProfilPic { get; set; }
        public string? Branch { get; set; }
        public string? Token { get; set; }
        public List<string> Roles { get; set; } = new();
    }

    public class ModelLoginResponse
    {
        public string? AuthToken { get; set; }
    }

    public class LoginRequestDto
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

    public class TokenUserInfo
    {
        public long UserId { get; set; }
        public string? Username { get; set; }   // email login identity
        public string? Name { get; set; }

        // Role based (legacy / UI use)
        public List<string> Roles { get; set; } = new();

        // Permission based (main authorization)
        public List<string> Permissions { get; set; } = new();
    }

    public class UserDetails
    {
        public string? UserName { get; set; }
        public string? Name { get; set; }
        //public string? Role { get; set; }
        public string? Password { get; set; }

        // Internal use (NOT in JWT)
        public long UserId { get; set; }
        public long OrganizationId { get; set; }
        public long BranchId { get; set; }
        public List<string> Roles { get; set; } = new();
        public List<string> Permissions { get; set; } = new();
    }
}
