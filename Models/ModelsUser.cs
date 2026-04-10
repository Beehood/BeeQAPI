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
        public string? Role { get; set; }
        public string? ProfilPic { get; set; }
        public string? Branch { get; set; }
        public string? Token { get; set; }
    }

    public class ModelLoginResponse
    {
        public string? AuthToken { get; set; }
    }

    public class LoginRequestDto
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class TokenUserInfo
    {
        public string? Username { get; set; }
        public string? Name { get; set; }
        public string? Role { get; set; }
        public string? ClientName { get; set; }
    }
}
