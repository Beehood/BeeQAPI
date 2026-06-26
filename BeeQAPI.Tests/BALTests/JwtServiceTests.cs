using BAL.Services;
using Microsoft.Extensions.Configuration;
using Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BeeQAPI.Tests.BALTests
{
    public class JwtServiceTests
    {
        private readonly JwtService _jwtService;
        private readonly IConfiguration _configuration;

        public JwtServiceTests()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "JwtSettings:Key", "ThisIsMySuperSecretKeyForJwtToken12345" },
                { "JwtSettings:Issuer", "BeeQAPI" },
                { "JwtSettings:Audience", "BeeQUsers" },
                { "JwtSettings:ExpiryInMinutes", "60" }
            };

            _configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _jwtService = new JwtService(_configuration);
        }

        [Fact]
        public async Task GenerateTokenAsync_Should_Return_Valid_Token()
        {
            // Arrange
            var user = new TokenUserInfo
            {
                Username = "swapna@test.com",
                Name = "Swapnalisa",
                OrganizationId = 1,
                BranchId = 2,
                CounterId = 3,
                Roles = new List<string> { "Super Admin", "Org Admin" },
                Permissions = new List<string> { "VIEW_USER", "CREATE_USER" }
            };

            // Act
            var token = await _jwtService.GenerateTokenAsync(user);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(token));
        }

        [Fact]
        public async Task GenerateTokenAsync_Should_Contain_Basic_User_Claims()
        {
            // Arrange
            var user = new TokenUserInfo
            {
                Username = "swapna@test.com",
                Name = "Swapnalisa",
                OrganizationId = 10,
                BranchId = 20,
                CounterId = 30,
                Roles = new List<string>(),
                Permissions = new List<string>()
            };

            // Act
            var token = await _jwtService.GenerateTokenAsync(user);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Assert
            Assert.Equal("swapna@test.com",
                jwtToken.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

            Assert.Equal("Swapnalisa",
                jwtToken.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Name)?.Value);

            Assert.Equal("10",
                jwtToken.Claims.FirstOrDefault(c => c.Type == "OrganizationId")?.Value);

            Assert.Equal("20",
                jwtToken.Claims.FirstOrDefault(c => c.Type == "BranchId")?.Value);

            Assert.Equal("30",
                jwtToken.Claims.FirstOrDefault(c => c.Type == "CounterId")?.Value);
        }

        [Fact]
        public async Task GenerateTokenAsync_Should_Contain_Role_Claims()
        {
            // Arrange
            var user = new TokenUserInfo
            {
                Username = "admin@test.com",
                Name = "Admin",
                OrganizationId = 1,
                BranchId = 1,
                CounterId = 1,
                Roles = new List<string> { "Super Admin", "Org Admin" },
                Permissions = new List<string>()
            };

            // Act
            var token = await _jwtService.GenerateTokenAsync(user);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var roles = jwtToken.Claims
                .Where(c => c.Type == System.Security.Claims.ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList();

            // Assert
            Assert.Contains("Super Admin", roles);
            Assert.Contains("Org Admin", roles);
        }

        [Fact]
        public async Task GenerateTokenAsync_Should_Contain_Permission_Claims()
        {
            // Arrange
            var user = new TokenUserInfo
            {
                Username = "admin@test.com",
                Name = "Admin",
                OrganizationId = 1,
                BranchId = 1,
                CounterId = 1,
                Roles = new List<string>(),
                Permissions = new List<string> { "VIEW_USER", "CREATE_USER" }
            };

            // Act
            var token = await _jwtService.GenerateTokenAsync(user);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var permissions = jwtToken.Claims
                .Where(c => c.Type == "permission")
                .Select(c => c.Value)
                .ToList();

            // Assert
            Assert.Contains("VIEW_USER", permissions);
            Assert.Contains("CREATE_USER", permissions);
        }

        [Fact]
        public async Task GenerateTokenAsync_Should_Work_When_Roles_And_Permissions_Are_Null()
        {
            // Arrange
            var user = new TokenUserInfo
            {
                Username = "user@test.com",
                Name = "User",
                OrganizationId = 5,
                BranchId = 6,
                CounterId = 7,
                Roles = null,
                Permissions = null
            };

            // Act
            var token = await _jwtService.GenerateTokenAsync(user);

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(token));
            Assert.Equal("user@test.com",
                jwtToken.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
        }
    }
}