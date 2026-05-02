using BAL.ContractIF;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BAL.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }

        public Task<string> GenerateTokenAsync(TokenUserInfo user)
        {
            var jwtSettings = _config.GetSection("JwtSettings");

            var claims = new List<Claim>
            {
               new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Name, user.Name ?? "")
            };

            // Add Roles
            if (user.Roles != null && user.Roles.Any())
            {
                claims.AddRange(
                    user.Roles.Select(role =>
                        new Claim(ClaimTypes.Role, role))
                );
            }

            // Add Permissions
            if (user.Permissions != null && user.Permissions.Any())
            {
                claims.AddRange(
                    user.Permissions.Select(permission =>
                        new Claim("permission", permission))
                );
            }

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            );

            var creds = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(jwtSettings["ExpiryInMinutes"])
                ),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Task.FromResult(tokenString);
        }
    }
}