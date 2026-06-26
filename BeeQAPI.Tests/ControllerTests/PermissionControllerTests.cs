using BAL.ContractIF;
using BeeQAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Models;
using System.Security.Claims;
using Xunit;

namespace BeeQAPI.Tests
{
    public class PermissionControllerTests
    {
        private readonly Mock<IBAL_Permission> _balMock;
        private readonly PermissionController _controller;

        public PermissionControllerTests()
        {
            _balMock = new Mock<IBAL_Permission>();
            _controller = new PermissionController(_balMock.Object);
        }

        private void SetUserClaims(List<string> roles, string? email)
        {
            var claims = new List<Claim>();

            if (roles != null)
            {
                claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, email));
            }

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = principal
                }
            };
        }

        // ========================
        // GET ALL
        // ========================
        [Fact]
        public async Task GetAll_Should_Return_Permission_List()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "admin@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<List<PermissionModel>>
            {
                IsSuccess = true,
                Result = new List<PermissionModel>
                {
                    new PermissionModel()
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

            _balMock.Verify(x => x.GetAll(request, roles, email, null), Times.Once);
        }

        // ========================
        // GET BY ID
        // ========================
        [Fact]
        public async Task GetById_Should_Return_Permission_By_Id()
        {
            // Arrange
            long id = 1;
            var roles = new List<string> { "Super Admin" };
            var email = "admin@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<PermissionModel>
            {
                IsSuccess = true,
                Result = new PermissionModel()
            };

            _balMock
                .Setup(x => x.GetById(id, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            _balMock.Verify(x => x.GetById(id, roles, email, null), Times.Once);
        }

        // ========================
        // CREATE
        // ========================
        [Fact]
        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new PermissionRequestDto
            {
                PermissionName = "View User",
                PermissionCode = "VIEW_USER",
                Module = "User"
            };

            var roles = new List<string> { "Super Admin" };
            var email = "admin@mail.com";

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
        // UPDATE
        // ========================
        [Fact]
        public async Task Update_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new PermissionRequestDto
            {
                PermissionId = 1,
                PermissionName = "Update User",
                PermissionCode = "UPDATE_USER",
                Module = "User"
            };

            var roles = new List<string> { "Super Admin" };
            var email = "admin@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _balMock
                .Setup(x => x.Update(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Update(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);

            _balMock.Verify(x => x.Update(request, roles, email, null), Times.Once);
        }

        // ========================
        // CHANGE STATUS
        // ========================
        [Fact]
        public async Task ChangeStatus_Should_Return_Success_When_PermissionId_Is_Valid()
        {
            // Arrange
            var request = new PermissionStatusRequestDto
            {
                PermissionId = 1
            };

            var roles = new List<string> { "Super Admin" };
            var email = "admin@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _balMock
                .Setup(x => x.ChangeStatus(request.PermissionId, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.ChangeStatus(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);

            _balMock.Verify(x => x.ChangeStatus(request.PermissionId, roles, email, null), Times.Once);
        }

        // ========================
        // DROPDOWN
        // ========================
        [Fact]
        public async Task GetDropdown_Should_Return_Permission_Dropdown()
        {
            // Arrange
            var email = "admin@mail.com";
            SetUserClaims(new List<string>(), email);

            var expectedResponse = new APIGetResponseModel<List<DropdownModel>>
            {
                IsSuccess = true,
                Result = new List<DropdownModel>
                {
                    new DropdownModel()
                }
            };

            _balMock
                .Setup(x => x.GetDropdown(email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetDropdown();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);

            _balMock.Verify(x => x.GetDropdown(email, null), Times.Once);
        }
    }
}