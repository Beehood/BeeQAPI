using BeeQAPI.Controllers;
using BAL.ContractIF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace BeeQAPI.Tests.ControllerTests
{
    public class ActivityLogControllerTests
    {
        private readonly Mock<IBAL_ActiveLog> _balMock;
        private readonly ActivityLogController _controller;

        public ActivityLogControllerTests()
        {
            _balMock = new Mock<IBAL_ActiveLog>();
            _controller = new ActivityLogController(_balMock.Object);
        }

        // =========================
        // HELPER: SET USER CLAIMS
        // =========================
        private void SetUserClaims(List<string> roles, string email)
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(email))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, email));
            }

            if (roles != null && roles.Any())
            {
                claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
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

        // =========================
        // GET ALL TESTS
        // =========================

        [Fact]
        public async Task GetAll_Should_Return_ActivityLogs_Successfully()
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

            var expectedResponse = new APIGetResponseModel<List<ActivityLogModel>>
            {
                IsSuccess = true,
                Result = new List<ActivityLogModel>
                {
                    new ActivityLogModel()
                },
                TotalRecords = 1
            };

            _balMock
                .Setup(x => x.GetAll(
                    request,
                    It.Is<List<string>>(r => r.Contains("Super Admin")),
                    email,
                    null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAll(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.Single(result.Result);

            _balMock.Verify(x => x.GetAll(
                request,
                It.Is<List<string>>(r => r.Contains("Super Admin")),
                email,
                null), Times.Once);
        }

        [Fact]
        public async Task GetAll_Should_Pass_Empty_Roles_When_No_Role_Claims()
        {
            var request = new PaginationRequestDto
            {
                PageNo = 1,
                PageSize = 10,
                SearchKey = ""
            };
            var email = "test@mail.com";
            SetUserClaims(new List<string>(), email);

            var expectedResponse = new APIGetResponseModel<List<ActivityLogModel>>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Access denied." }
            };

            _balMock
                .Setup(x => x.GetAll(
                    request,
                    It.Is<List<string>>(r => r.Count == 0),
                    email,
                    null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAll(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);

            _balMock.Verify(x => x.GetAll(
                request,
                It.Is<List<string>>(r => r.Count == 0),
                email,
                null), Times.Once);
        }

        // =========================
        // GET BY ID TESTS
        // =========================

        [Fact]
        public async Task GetById_Should_Return_ActivityLog_By_Id()
        {
            // Arrange
            long id = 1;
            var roles = new List<string> { "Org Admin" };
            var email = "orgadmin@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<ActivityLogModel>
            {
                IsSuccess = true,
                Result = new ActivityLogModel()
            };

            _balMock
                .Setup(x => x.GetById(
                    id,
                    It.Is<List<string>>(r => r.Contains("Org Admin")),
                    email,
                    null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);

            _balMock.Verify(x => x.GetById(
                id,
                It.Is<List<string>>(r => r.Contains("Org Admin")),
                email,
                null), Times.Once);
        }

        [Fact]
        public async Task GetById_Should_Pass_Null_Email_When_Email_Claim_Missing()
        {
            // Arrange
            long id = 5;
            var roles = new List<string> { "Branch Admin" };

            SetUserClaims(roles, null);

            var expectedResponse = new APIGetResponseModel<ActivityLogModel>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Invalid user." }
            };

            _balMock
                .Setup(x => x.GetById(
                    id,
                    It.Is<List<string>>(r => r.Contains("Branch Admin")),
                    null,
                    null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid user.", result.ErrorMsgs);

            _balMock.Verify(x => x.GetById(
                id,
                It.Is<List<string>>(r => r.Contains("Branch Admin")),
                null,
                null), Times.Once);
        }

        // =========================
        // CREATE TESTS
        // =========================

        [Fact]
        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new ActivityLogRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "admin@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _balMock
                .Setup(x => x.Create(
                    request,
                    It.Is<List<string>>(r => r.Contains("Super Admin")),
                    email,
                    null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Create(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);

            _balMock.Verify(x => x.Create(
                request,
                It.Is<List<string>>(r => r.Contains("Super Admin")),
                email,
                null), Times.Once);
        }

        [Fact]
        public async Task Create_Should_Return_Error_When_BAL_Returns_Failure()
        {
            // Arrange
            var request = new ActivityLogRequestDto();
            var roles = new List<string> { "Org Admin" };
            var email = "org@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Validation failed." }
            };

            _balMock
                .Setup(x => x.Create(
                    request,
                    It.Is<List<string>>(r => r.Contains("Org Admin")),
                    email,
                    null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Create(request);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Validation failed.", result.ErrorMsgs);

            _balMock.Verify(x => x.Create(
                request,
                It.Is<List<string>>(r => r.Contains("Org Admin")),
                email,
                null), Times.Once);
        }
    }
}