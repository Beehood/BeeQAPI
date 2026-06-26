using BAL.ContractIF;
using BeeQAPI.Controllers;
using BeeQAPI.Realtime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Models;
using System.Security.Claims;
using Xunit;

namespace BeeQAPI.Tests
{
    public class CounterPanelControllerTests
    {
        private readonly Mock<IBAL_CounterPanel> _balMock;
        private readonly Mock<IHubContext<QueueHub>> _hubContextMock;
        private readonly Mock<IHubClients> _hubClientsMock;
        private readonly Mock<IClientProxy> _clientProxyMock;
        private readonly CounterPanelController _controller;

        public CounterPanelControllerTests()
        {
            _balMock = new Mock<IBAL_CounterPanel>();
            _hubContextMock = new Mock<IHubContext<QueueHub>>();
            _hubClientsMock = new Mock<IHubClients>();
            _clientProxyMock = new Mock<IClientProxy>();

            _hubClientsMock
                .Setup(x => x.Group(It.IsAny<string>()))
                .Returns(_clientProxyMock.Object);

            _hubContextMock
                .Setup(x => x.Clients)
                .Returns(_hubClientsMock.Object);

            _controller = new CounterPanelController(_balMock.Object, _hubContextMock.Object);
        }

        // =========================================================
        // Helper: Set User Claims
        // =========================================================
        private void SetUserClaims(List<string> roles, string? email, string? branchId = "1")
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(email))
                claims.Add(new Claim(ClaimTypes.NameIdentifier, email));

            if (!string.IsNullOrWhiteSpace(branchId))
                claims.Add(new Claim("BranchId", branchId));

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
        // DASHBOARD
        // =========================================================
        [Fact]
        public async Task GetDashboard_Should_Return_Dashboard_Successfully()
        {
            // Arrange
            var request = new CounterPanelActionRequestDto();
            var roles = new List<string> { "Counter Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<CounterPanelDashboardModel>
            {
                IsSuccess = true,
                Result = new CounterPanelDashboardModel()
            };

            _balMock
                .Setup(x => x.GetDashboard(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetDashboard(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task GetDashboard_Should_Pass_Empty_Roles_When_No_Role_Claims()
        {
            // Arrange
            var request = new CounterPanelActionRequestDto();
            var email = "test@mail.com";

            SetUserClaims(new List<string>(), email);

            var expectedResponse = new APIGetResponseModel<CounterPanelDashboardModel>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Access denied." }
            };

            _balMock
                .Setup(x => x.GetDashboard(request, It.Is<List<string>>(r => r.Count == 0), email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetDashboard(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        // =========================================================
        // CALL NEXT TOKEN
        // =========================================================
        [Fact]
        public async Task CallNextToken_Should_Return_Success_And_Send_QueueUpdated()
        {
            // Arrange
            var request = new CounterPanelActionRequestDto();
            var roles = new List<string> { "Counter Admin" };
            var email = "test@mail.com";
            var branchId = "5";

            SetUserClaims(roles, email, branchId);

            var expectedResponse = new APIGetResponseModel<CallNextTokenResponseDto>
            {
                IsSuccess = true,
                Result = new CallNextTokenResponseDto()
            };

            _balMock
                .Setup(x => x.CallNextToken(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CallNextToken(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            _hubClientsMock.Verify(x => x.Group(branchId), Times.Once);

            _clientProxyMock.Verify(x =>
                x.SendCoreAsync(
                    "QueueUpdated",
                    It.Is<object[]>(o => o.Length == 0),
                    default),
                Times.Once);
        }

        [Fact]
        public async Task CallNextToken_Should_Not_Send_QueueUpdated_When_Failed()
        {
            // Arrange
            var request = new CounterPanelActionRequestDto();
            var roles = new List<string> { "Counter Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email, "2");

            var expectedResponse = new APIGetResponseModel<CallNextTokenResponseDto>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Failed" }
            };

            _balMock
                .Setup(x => x.CallNextToken(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CallNextToken(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);

            _clientProxyMock.Verify(x =>
                x.SendCoreAsync(
                    "QueueUpdated",
                    It.IsAny<object[]>(),
                    default),
                Times.Never);
        }

        // =========================================================
        // START SERVICE
        // =========================================================
        [Fact]
        public async Task StartService_Should_Return_Success_And_Send_QueueUpdated()
        {
            // Arrange
            var request = new CounterPanelActionRequestDto();
            var roles = new List<string> { "Counter Admin" };
            var email = "test@mail.com";
            var branchId = "3";

            SetUserClaims(roles, email, branchId);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _balMock
                .Setup(x => x.StartService(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.StartService(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);

            _hubClientsMock.Verify(x => x.Group(branchId), Times.Once);

            _clientProxyMock.Verify(x =>
                x.SendCoreAsync(
                    "QueueUpdated",
                    It.Is<object[]>(o => o.Length == 0),
                    default),
                Times.Once);
        }

        [Fact]
        public async Task StartService_Should_Not_Send_QueueUpdated_When_Failed()
        {
            // Arrange
            var request = new CounterPanelActionRequestDto();
            var roles = new List<string> { "Counter Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email, "3");

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Failed" }
            };

            _balMock
                .Setup(x => x.StartService(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.StartService(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);

            _clientProxyMock.Verify(x =>
                x.SendCoreAsync(
                    "QueueUpdated",
                    It.IsAny<object[]>(),
                    default),
                Times.Never);
        }

        // =========================================================
        // COMPLETE SERVICE
        // =========================================================
        [Fact]
        public async Task CompleteService_Should_Return_Success_And_Send_QueueUpdated()
        {
            // Arrange
            var request = new CounterPanelActionRequestDto();
            var roles = new List<string> { "Counter Admin" };
            var email = "test@mail.com";
            var branchId = "4";

            SetUserClaims(roles, email, branchId);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _balMock
                .Setup(x => x.CompleteService(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CompleteService(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);

            _hubClientsMock.Verify(x => x.Group(branchId), Times.Once);

            _clientProxyMock.Verify(x =>
                x.SendCoreAsync(
                    "QueueUpdated",
                    It.Is<object[]>(o => o.Length == 0),
                    default),
                Times.Once);
        }

        [Fact]
        public async Task CompleteService_Should_Not_Send_QueueUpdated_When_Failed()
        {
            // Arrange
            var request = new CounterPanelActionRequestDto();
            var roles = new List<string> { "Counter Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email, "4");

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Failed" }
            };

            _balMock
                .Setup(x => x.CompleteService(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CompleteService(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);

            _clientProxyMock.Verify(x =>
                x.SendCoreAsync(
                    "QueueUpdated",
                    It.IsAny<object[]>(),
                    default),
                Times.Never);
        }

        // =========================================================
        // SKIP TOKEN
        // =========================================================
        [Fact]
        public async Task SkipToken_Should_Return_Success_And_Send_QueueUpdated()
        {
            // Arrange
            var request = new CounterPanelActionRequestDto();
            var roles = new List<string> { "Counter Admin" };
            var email = "test@mail.com";
            var branchId = "7";

            SetUserClaims(roles, email, branchId);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _balMock
                .Setup(x => x.SkipToken(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.SkipToken(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            _hubClientsMock.Verify(x => x.Group(branchId), Times.Once);

            _clientProxyMock.Verify(x =>
                x.SendCoreAsync(
                    "QueueUpdated",
                    It.Is<object[]>(o => o.Length == 0),
                    default),
                Times.Once);
        }

        [Fact]
        public async Task SkipToken_Should_Not_Send_QueueUpdated_When_Failed()
        {
            // Arrange
            var request = new CounterPanelActionRequestDto();
            var roles = new List<string> { "Counter Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email, "7");

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Failed" }
            };

            _balMock
                .Setup(x => x.SkipToken(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.SkipToken(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);

            _clientProxyMock.Verify(x =>
                x.SendCoreAsync(
                    "QueueUpdated",
                    It.IsAny<object[]>(),
                    default),
                Times.Never);
        }

        // =========================================================
        // RECALL TOKEN
        // =========================================================
        [Fact]
        public async Task RecallToken_Should_Return_Success_And_Send_QueueUpdated()
        {
            // Arrange
            var request = new CounterPanelActionRequestDto();
            var roles = new List<string> { "Counter Admin" };
            var email = "test@mail.com";
            var branchId = "9";

            SetUserClaims(roles, email, branchId);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _balMock
                .Setup(x => x.RecallToken(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.RecallToken(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            _hubClientsMock.Verify(x => x.Group(branchId), Times.Once);

            _clientProxyMock.Verify(x =>
                x.SendCoreAsync(
                    "QueueUpdated",
                    It.Is<object[]>(o => o.Length == 0),
                    default),
                Times.Once);
        }

        [Fact]
        public async Task RecallToken_Should_Not_Send_QueueUpdated_When_Failed()
        {
            // Arrange
            var request = new CounterPanelActionRequestDto();
            var roles = new List<string> { "Counter Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email, "9");

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Failed" }
            };

            _balMock
                .Setup(x => x.RecallToken(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.RecallToken(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);

            _clientProxyMock.Verify(x =>
                x.SendCoreAsync(
                    "QueueUpdated",
                    It.IsAny<object[]>(),
                    default),
                Times.Never);
        }
    }
}