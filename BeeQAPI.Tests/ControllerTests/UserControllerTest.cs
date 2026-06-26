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
    public class UserControllerTests
    {
        private readonly Mock<IBAL_User> _balMock;
        private readonly UserController _controller;


    public UserControllerTests()
        {
            _balMock = new Mock<IBAL_User>();
            _controller = new UserController(_balMock.Object);
        }
    
        private void SetUserClaims(List<string> roles, string? email)
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(email))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, email));
            }

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
        public async Task GetAll_Should_Return_User_List()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "admin@test.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<List<UserModel>>
            {
                IsSuccess = true,
                Result = new List<UserModel>
            {
                new UserModel()
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
        // GET BY ID
        // ========================
        [Fact]
        public async Task GetById_Should_Return_User_By_Id()
        {
            // Arrange
            long id = 1;
            var roles = new List<string> { "Super Admin" };
            var email = "admin@test.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<UserModel>
            {
                IsSuccess = true,
                Result = new UserModel()
            };

            _balMock
                .Setup(x => x.GetById(id, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);

            _balMock.Verify(x => x.GetById(id, roles, email, null), Times.Once);
        }

        // ========================
        // CREATE
        // ========================
        [Fact]
        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new UserRequestDto
            {
                Name = "Test User",
                Email = "test@test.com",
                Password = "123456",
                RoleId = 1
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
        // UPDATE
        // ========================
        [Fact]
        public async Task Update_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new UserRequestDto
            {
                UserId = 1,
                Name = "Updated User",
                Email = "updated@test.com",
                RoleId = 1
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
        public async Task ChangeStatus_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new UserStatusRequestDto
            {
                UserId = 1
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
                .Setup(x => x.ChangeStatus(request.UserId, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.ChangeStatus(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);

            _balMock.Verify(x => x.ChangeStatus(request.UserId, roles, email, null), Times.Once);
        }

        // ========================
        // DROPDOWN
        // ========================
        [Fact]
        public async Task GetDropdown_Should_Return_Dropdown_List()
        {
            // Arrange
            var email = "admin@test.com";

            SetUserClaims(new List<string> { "Super Admin" }, email);

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
            Assert.Single(result.Result);

            _balMock.Verify(x => x.GetDropdown(email, null), Times.Once);
        }

        // ========================
        // EXTRA CLAIM TESTS
        // ========================
        [Fact]
        public async Task GetAll_Should_Pass_Empty_Roles_When_No_Role_Claims()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var email = "admin@test.com";

            SetUserClaims(new List<string>(), email);

            var expectedResponse = new APIGetResponseModel<List<UserModel>>
            {
                IsSuccess = true,
                Result = new List<UserModel>()
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
            var request = new UserRequestDto
            {
                Name = "Test User",
                Email = "test@test.com",
                Password = "123456",
                RoleId = 1
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
