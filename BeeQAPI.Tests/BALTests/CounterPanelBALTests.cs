using Xunit;
using Moq;
using BAL.Services;
using DAL.ContractIF;
using Microsoft.Extensions.Logging;
using Models;
using System.Data;

namespace BeeQAPI.Tests.BALTests
{
    public class CounterPanelBALTests
    {
        private readonly Mock<IDAL_CounterPanel> _mockDal;
        private readonly Mock<ILogger<BAL_CounterPanel>> _mockLogger;
        private readonly BAL_CounterPanel _bal;

        public CounterPanelBALTests()
        {
            _mockDal = new Mock<IDAL_CounterPanel>();
            _mockLogger = new Mock<ILogger<BAL_CounterPanel>>();
            _bal = new BAL_CounterPanel(_mockDal.Object, _mockLogger.Object);
        }

        // =========================
        // GET DASHBOARD
        // =========================
        [Fact]
        public async Task GetDashboard_Should_Return_AccessDenied_When_Roles_Are_Empty()
        {
            // Arrange
            var request = new CounterPanelActionRequestDto();
            var roles = new List<string>();

            // Act
            var result = await _bal.GetDashboard(request, roles, "test@test.com");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetDashboard_Should_Return_Data_When_Roles_Are_Valid()
        {
            // Arrange
            _mockDal.Setup(x => x.GetDashboard(
                    It.IsAny<CounterPanelActionRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<CounterPanelDashboardModel>
                {
                    IsSuccess = true,
                    Result = new CounterPanelDashboardModel()
                });

            // Act
            var result = await _bal.GetDashboard(
                new CounterPanelActionRequestDto(),
                new List<string> { "Branch Admin" },
                "test@test.com");

            // Assert
            Assert.True(result.IsSuccess);
        }

        // =========================
        // CALL NEXT TOKEN
        // =========================
        [Fact]
        public async Task CallNextToken_Should_Return_AccessDenied_When_Roles_Are_Empty()
        {
            // Arrange
            var request = new CounterPanelActionRequestDto();

            // Act
            var result = await _bal.CallNextToken(
                request,
                new List<string>(),
                "test@test.com");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task CallNextToken_Should_Return_Error_When_CounterId_Is_Invalid()
        {
            // Arrange
            var request = new CounterPanelActionRequestDto
            {
                CounterId = 0
            };

            // Act
            var result = await _bal.CallNextToken(
                request,
                new List<string> { "Branch Admin" },
                "test@test.com");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Counter required.", result.ErrorMsgs);
        }

        [Fact]
        public async Task CallNextToken_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new CounterPanelActionRequestDto
            {
                CounterId = 1
            };

            _mockDal.Setup(x => x.CallNextToken(
                    It.IsAny<CounterPanelActionRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<CallNextTokenResponseDto>
                {
                    IsSuccess = true,
                    Result = new CallNextTokenResponseDto()
                });

            // Act
            var result = await _bal.CallNextToken(
                request,
                new List<string> { "Branch Admin" },
                "test@test.com");

            // Assert
            Assert.True(result.IsSuccess);
        }

        // =========================
        // START SERVICE
        // =========================
        [Fact]
        public async Task StartService_Should_Return_AccessDenied_When_Roles_Are_Empty()
        {
            var result = await _bal.StartService(
                new CounterPanelActionRequestDto(),
                new List<string>(),
                "test@test.com");

            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task StartService_Should_Return_Success_When_Roles_Are_Valid()
        {
            _mockDal.Setup(x => x.StartService(
                    It.IsAny<CounterPanelActionRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<int>
                {
                    IsSuccess = true,
                    Result = 1
                });

            var result = await _bal.StartService(
                new CounterPanelActionRequestDto(),
                new List<string> { "Branch Admin" },
                "test@test.com");

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }

        // =========================
        // COMPLETE SERVICE
        // =========================
        [Fact]
        public async Task CompleteService_Should_Return_AccessDenied_When_Roles_Are_Empty()
        {
            var result = await _bal.CompleteService(
                new CounterPanelActionRequestDto(),
                new List<string>(),
                "test@test.com");

            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task CompleteService_Should_Return_Success_When_Roles_Are_Valid()
        {
            _mockDal.Setup(x => x.CompleteService(
                    It.IsAny<CounterPanelActionRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<int>
                {
                    IsSuccess = true,
                    Result = 1
                });

            var result = await _bal.CompleteService(
                new CounterPanelActionRequestDto(),
                new List<string> { "Branch Admin" },
                "test@test.com");

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }

        // =========================
        // SKIP TOKEN
        // =========================
        [Fact]
        public async Task SkipToken_Should_Return_AccessDenied_When_Roles_Are_Empty()
        {
            var result = await _bal.SkipToken(
                new CounterPanelActionRequestDto(),
                new List<string>(),
                "test@test.com");

            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task SkipToken_Should_Return_Success_When_Roles_Are_Valid()
        {
            _mockDal.Setup(x => x.SkipToken(
                    It.IsAny<CounterPanelActionRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<int>
                {
                    IsSuccess = true,
                    Result = 1
                });

            var result = await _bal.SkipToken(
                new CounterPanelActionRequestDto(),
                new List<string> { "Branch Admin" },
                "test@test.com");

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }

        // =========================
        // RECALL TOKEN
        // =========================
        [Fact]
        public async Task RecallToken_Should_Return_AccessDenied_When_Roles_Are_Empty()
        {
            var result = await _bal.RecallToken(
                new CounterPanelActionRequestDto(),
                new List<string>(),
                "test@test.com");

            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task RecallToken_Should_Return_Success_When_Roles_Are_Valid()
        {
            _mockDal.Setup(x => x.RecallToken(
                    It.IsAny<CounterPanelActionRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<int>
                {
                    IsSuccess = true,
                    Result = 1
                });

            var result = await _bal.RecallToken(
                new CounterPanelActionRequestDto(),
                new List<string> { "Branch Admin" },
                "test@test.com");

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }
    }
}