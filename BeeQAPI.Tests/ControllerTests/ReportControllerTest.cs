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
    public class ReportControllerTests
    {
        private readonly Mock<IBAL_Report> _balMock;
        private readonly ReportController _controller;

        public ReportControllerTests()
        {
            _balMock = new Mock<IBAL_Report>();
            _controller = new ReportController(_balMock.Object);
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

        [Fact]
        public async Task GetAll_Should_Return_Report_List()
        {
            // Arrange
            var request = new ReportRequestDto
            {
                Action = "TOKEN_SUMMARY"
            };

            var roles = new List<string> { "Super Admin" };
            var email = "admin@test.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<List<ReportModel>>
            {
                IsSuccess = true,
                Result = new List<ReportModel>
            {
                new ReportModel()
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

        [Fact]
        public async Task GetAll_Should_Pass_Empty_Roles_When_No_Role_Claims()
        {
            // Arrange
            var request = new ReportRequestDto
            {
                Action = "TOKEN_SUMMARY"
            };

            var roles = new List<string>();
            var email = "admin@test.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<List<ReportModel>>
            {
                IsSuccess = true,
                Result = new List<ReportModel>()
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
        public async Task GetAll_Should_Pass_Null_Email_When_NameIdentifier_Not_Present()
        {
            // Arrange
            var request = new ReportRequestDto
            {
                Action = "TOKEN_SUMMARY"
            };

            var roles = new List<string> { "Super Admin" };

            var claims = roles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var user = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            var expectedResponse = new APIGetResponseModel<List<ReportModel>>
            {
                IsSuccess = true,
                Result = new List<ReportModel>()
            };

            _balMock
                .Setup(x => x.GetAll(request, roles, null, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAll(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            _balMock.Verify(x => x.GetAll(request, roles, null, null), Times.Once);
        }


    }
}
