using BAL.ContractIF;
using BeeQAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Models;
using System.Security.Claims;
using Xunit;

namespace BeeQAPI.Tests.Controllers
{
    public class RolePermissionControllerTests
    {
        private readonly Mock<IBAL_RolePermission> _balMock;
        private readonly RolePermissionController _controller;

    public RolePermissionControllerTests()
        {
            _balMock = new Mock<IBAL_RolePermission>();
            _controller = new RolePermissionController(_balMock.Object);
        }

        private void SetUserClaims(List<string> roles, string email)
        {
            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, email)
        };

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var user = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };
        }

        // ========================
        // GET ALL
        // ========================
        [Fact]
        public async Task GetAll_Should_Return_RolePermission_List()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "admin@test.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<List<RolePermissionModel>>
            {
                IsSuccess = true,
                Result = new List<RolePermissionModel>
            {
                new RolePermissionModel()
            }
            };

            _balMock
                .Setup(x => x.GetAll(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAll(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.Single(result.Result);

            _balMock.Verify(x => x.GetAll(request, roles, email, null), Times.Once);
        }

        // ========================
        // GET BY ROLE ID
        // ========================
        [Fact]
        public async Task GetByRoleId_Should_Return_RolePermissions_By_RoleId()
        {
            // Arrange
            long roleId = 1;
            var roles = new List<string> { "Super Admin" };
            var email = "admin@test.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<List<RolePermissionModel>>
            {
                IsSuccess = true,
                Result = new List<RolePermissionModel>
            {
                new RolePermissionModel()
            }
            };

            _balMock
                .Setup(x => x.GetByRoleId(roleId, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetByRoleId(roleId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.Single(result.Result);

            _balMock.Verify(x => x.GetByRoleId(roleId, roles, email, null), Times.Once);
        }

        // ========================
        // CREATE
        // ========================
        [Fact]
        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new RolePermissionRequestDto
            {
                RoleId = 1,
                PermissionId = 2
            };

            var roles = new List<string> { "Super Admin" };
            var email = "admin@test.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _balMock
                .Setup(x => x.Create(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Create(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);

            _balMock.Verify(x => x.Create(request, roles, email, null), Times.Once);
        }

        // ========================
        // BULK ASSIGN
        // ========================
        [Fact]
        public async Task BulkAssign_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new RolePermissionRequestDto
            {
                RoleId = 1,
                PermissionIds = "1,2,3"
            };

            var roles = new List<string> { "Super Admin" };
            var email = "admin@test.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _balMock
                .Setup(x => x.BulkAssign(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.BulkAssign(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);

            _balMock.Verify(x => x.BulkAssign(request, roles, email, null), Times.Once);
        }

        // ========================
        // DELETE
        // ========================
        [Fact]
        public async Task Delete_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new RolePermissionRequestDto
            {
                RoleId = 1,
                PermissionId = 2
            };

            var roles = new List<string> { "Super Admin" };
            var email = "admin@test.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _balMock
                .Setup(x => x.Delete(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Delete(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);

            _balMock.Verify(x => x.Delete(request, roles, email, null), Times.Once);
        }

        // ========================
        // EXTRA CLAIM TESTS
        // ========================
        [Fact]
        public async Task GetAll_Should_Pass_Empty_Roles_When_No_Role_Claims()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var roles = new List<string>();
            var email = "admin@test.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<List<RolePermissionModel>>
            {
                IsSuccess = true,
                Result = new List<RolePermissionModel>()
            };

            _balMock
                .Setup(x => x.GetAll(request, It.Is<List<string>>(r => r.Count == 0), email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAll(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            _balMock.Verify(x => x.GetAll(request, It.Is<List<string>>(r => r.Count == 0), email, null), Times.Once);
        }

        [Fact]
        public async Task Create_Should_Pass_Null_Email_When_NameIdentifier_Not_Present()
        {
            // Arrange
            var request = new RolePermissionRequestDto
            {
                RoleId = 1,
                PermissionId = 2
            };

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, "Super Admin")
        };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var user = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            var roles = new List<string> { "Super Admin" };

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _balMock
                .Setup(x => x.Create(request, roles, null, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Create(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            _balMock.Verify(x => x.Create(request, roles, null, null), Times.Once);
        }
    }

}
