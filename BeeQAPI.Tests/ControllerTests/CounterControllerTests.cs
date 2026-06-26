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
    public class CounterControllerTests
    {
        private readonly Mock<IBAL_Counter> _balMock;
        private readonly CounterController _controller;

        public CounterControllerTests()
        {
            _balMock = new Mock<IBAL_Counter>();
            _controller = new CounterController(_balMock.Object);
        }

        // =========================================================
        // Helper: Set User Claims
        // =========================================================
        private void SetUserClaims(List<string> roles, string? email)
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(email))
                claims.Add(new Claim(ClaimTypes.NameIdentifier, email));

            if (roles != null && roles.Any())
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

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

        // =========================================================
        // GET ALL
        // =========================================================
        [Fact]
        public async Task GetAll_Should_Return_Counter_List_Successfully()
        {
            // Arrange
            var request = new PaginationRequestDto
            {
                PageNo = 1,
                PageSize = 10,
                SearchKey = ""
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<List<CounterModel>>
            {
                IsSuccess = true,
                Result = new List<CounterModel>
                {
                    new CounterModel(),
                    new CounterModel()
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
            Assert.Equal(2, result.Result.Count);
        }

        [Fact]
        public async Task GetAll_Should_Pass_Empty_Roles_When_No_Role_Claims()
        {
            // Arrange
            var request = new PaginationRequestDto
            {
                PageNo = 1,
                PageSize = 10,
                SearchKey = ""
            };

            var roles = new List<string>();
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<List<CounterModel>>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Access denied." }
            };

            _balMock
                .Setup(x => x.GetAll(request, It.Is<List<string>>(r => r.Count == 0), email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAll(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        // =========================================================
        // GET BY ID
        // =========================================================
        [Fact]
        public async Task GetById_Should_Return_Counter_By_Id()
        {
            // Arrange
            long id = 1;
            var roles = new List<string> { "Super Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<CounterModel>
            {
                IsSuccess = true,
                Result = new CounterModel()
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
        }

        [Fact]
        public async Task GetById_Should_Pass_Empty_Roles_When_No_Role_Claims()
        {
            // Arrange
            long id = 5;
            var roles = new List<string>();
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<CounterModel>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Access denied." }
            };

            _balMock
                .Setup(x => x.GetById(id, It.Is<List<string>>(r => r.Count == 0), email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        // =========================================================
        // CREATE
        // =========================================================
        [Fact]
        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new CounterRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@mail.com";

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
        }

        [Fact]
        public async Task Create_Should_Pass_Empty_Roles_When_No_Role_Claims()
        {
            // Arrange
            var request = new CounterRequestDto();
            var roles = new List<string>();
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Access denied." }
            };

            _balMock
                .Setup(x => x.Create(request, It.Is<List<string>>(r => r.Count == 0), email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Create(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        // =========================================================
        // UPDATE
        // =========================================================
        [Fact]
        public async Task Update_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new CounterRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@mail.com";

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
        }

        [Fact]
        public async Task Update_Should_Pass_Empty_Roles_When_No_Role_Claims()
        {
            // Arrange
            var request = new CounterRequestDto();
            var roles = new List<string>();
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Access denied." }
            };

            _balMock
                .Setup(x => x.Update(request, It.Is<List<string>>(r => r.Count == 0), email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Update(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        // =========================================================
        // CHANGE STATUS
        // =========================================================
        [Fact]
        public async Task ChangeStatus_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new CounterStatusRequestDto
            {
                CounterId = 10
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _balMock
                .Setup(x => x.ChangeStatus(request.CounterId, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.ChangeStatus(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }

        [Fact]
        public async Task ChangeStatus_Should_Pass_Empty_Roles_When_No_Role_Claims()
        {
            // Arrange
            var request = new CounterStatusRequestDto
            {
                CounterId = 10
            };

            var roles = new List<string>();
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Access denied." }
            };

            _balMock
                .Setup(x => x.ChangeStatus(request.CounterId, It.Is<List<string>>(r => r.Count == 0), email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.ChangeStatus(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        // =========================================================
        // DROPDOWN
        // =========================================================
        [Fact]
        public async Task GetDropdown_Should_Return_Dropdown_List_Successfully()
        {
            // Arrange
            var email = "test@mail.com";
            SetUserClaims(new List<string> { "Super Admin" }, email);

            var expectedResponse = new APIGetResponseModel<List<DropdownModel>>
            {
                IsSuccess = true,
                Result = new List<DropdownModel>
                {
                    new DropdownModel(),
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
            Assert.Equal(2, result.Result.Count);
        }

        [Fact]
        public async Task GetDropdown_Should_Pass_Null_Email_When_Claim_Missing()
        {
            // Arrange
            SetUserClaims(new List<string> { "Super Admin" }, null);

            var expectedResponse = new APIGetResponseModel<List<DropdownModel>>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Invalid user." }
            };

            _balMock
                .Setup(x => x.GetDropdown(null, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetDropdown();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid user.", result.ErrorMsgs);
        }
    }
}