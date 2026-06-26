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
    public class AppointmentControllerTests
    {
        private readonly Mock<IBAL_Appointment> _balMock;
        private readonly AppointmentController _controller;

        public AppointmentControllerTests()
        {
            _balMock = new Mock<IBAL_Appointment>();
            _controller = new AppointmentController(_balMock.Object);
        }

        // =========================================================
        // Helper: Set User Claims in ControllerContext
        // =========================================================
        private void SetUserClaims(List<string> roles, string email, string? branchId = null)
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(email))
                claims.Add(new Claim(ClaimTypes.NameIdentifier, email));

            if (roles != null)
            {
                claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            }

            if (!string.IsNullOrWhiteSpace(branchId))
                claims.Add(new Claim("BranchId", branchId));

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
        public async Task GetAll_Should_Return_Appointments_Successfully()
        {
            // Arrange
            var request = new AppointmentRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<List<AppointmentModel>>
            {
                IsSuccess = true,
                Result = new List<AppointmentModel>
                {
                    new AppointmentModel(),
                    new AppointmentModel()
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
            Assert.Equal(2, result.Result.Count);
        }

        [Fact]
        public async Task GetAll_Should_Pass_Empty_Roles_When_No_Role_Claims()
        {
            // Arrange
            var request = new AppointmentRequestDto();
            var roles = new List<string>();
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<List<AppointmentModel>>
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
        public async Task GetById_Should_Return_Appointment_By_Id()
        {
            // Arrange
            long id = 1;
            var roles = new List<string> { "Super Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<AppointmentModel>
            {
                IsSuccess = true,
                Result = new AppointmentModel()
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

            var expectedResponse = new APIGetResponseModel<AppointmentModel>
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
            var request = new AppointmentRequestDto();
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
            var request = new AppointmentRequestDto();
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
            var request = new AppointmentRequestDto();
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
            var request = new AppointmentRequestDto();
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
            var request = new AppointmentStatusRequestDto
            {
                AppointmentId = 10,
                Status = 1
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@mail.com";
            var branchId = "2";

            SetUserClaims(roles, email, branchId);

            // optional authorization header because controller reads it
            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer test-token";

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _balMock
                .Setup(x => x.ChangeStatus(request.AppointmentId, request.Status, roles, email, null))
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
            var request = new AppointmentStatusRequestDto
            {
                AppointmentId = 10,
                Status = 0
            };

            var roles = new List<string>();
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            _controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer test-token";

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Access denied." }
            };

            _balMock
                .Setup(x => x.ChangeStatus(request.AppointmentId, request.Status, It.Is<List<string>>(r => r.Count == 0), email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.ChangeStatus(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }
    }
}