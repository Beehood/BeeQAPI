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
    public class DashboardControllerTests
    {
        private readonly Mock<IBAL_Dashboard> _balMock;
        private readonly DashboardController _controller;

        public DashboardControllerTests()
        {
            _balMock = new Mock<IBAL_Dashboard>();
            _controller = new DashboardController(_balMock.Object);
        }

        private void SetUserClaims(List<string> roles, string email)
        {
            var claims = new List<Claim>();

            if (roles != null)
            {
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
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
        // DASHBOARD
        // ========================

        [Fact]
        public async Task GetDashboard_Should_Return_Dashboard_Data_Successfully()
        {
            // Arrange
            var request = new DashboardRequestDto();

            var roles = new List<string> { "Super Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<DashboardModel>
            {
                IsSuccess = true,
                Result = new DashboardModel()
            };

            _balMock
                .Setup(x => x.GetDashboard(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetDashboard(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(expectedResponse.Result, result.Result);

            _balMock.Verify(x => x.GetDashboard(request, roles, email, null), Times.Once);
        }

        [Fact]
        public async Task GetDashboard_Should_Pass_Empty_Roles_When_No_Role_Claims()
        {
            // Arrange
            var request = new DashboardRequestDto();

            var roles = new List<string>();
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<DashboardModel>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Access denied." }
            };

            _balMock
                .Setup(x => x.GetDashboard(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetDashboard(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);

            _balMock.Verify(x => x.GetDashboard(request, roles, email, null), Times.Once);
        }

        [Fact]
        public async Task GetDashboard_Should_Pass_Null_Email_When_NameIdentifier_Claim_Is_Missing()
        {
            // Arrange
            var request = new DashboardRequestDto();

            var roles = new List<string> { "Super Admin" };
            string email = null;

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<DashboardModel>
            {
                IsSuccess = true,
                Result = new DashboardModel()
            };

            _balMock
                .Setup(x => x.GetDashboard(request, roles, null, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetDashboard(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            _balMock.Verify(x => x.GetDashboard(request, roles, null, null), Times.Once);
        }
    }
}