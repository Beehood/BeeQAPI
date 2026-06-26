using BAL.Services;
using DAL.ContractIF;
using Models;
using Moq;
using System.Data;
using Xunit;

namespace BeeQAPI.Tests.BALTests
{
    public class DashboardBALTests
    {
        private readonly Mock<IDAL_Dashboard> _mockDal;
        private readonly BAL_Dashboard _bal;

        public DashboardBALTests()
        {
            _mockDal = new Mock<IDAL_Dashboard>();
            _bal = new BAL_Dashboard(_mockDal.Object);
        }

        [Fact]
        public async Task GetDashboard_Should_Return_AccessDenied_When_UserHasNoValidRole()
        {
            // Arrange
            var request = new DashboardRequestDto();
            var roles = new List<string> { "User" };
            var email = "test@test.com";

            // Act
            var result = await _bal.GetDashboard(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetDashboard_Should_Return_InvalidUser_When_Email_Is_Empty()
        {
            // Arrange
            var request = new DashboardRequestDto();
            var roles = new List<string> { "Super Admin" };
            string email = "";

            // Act
            var result = await _bal.GetDashboard(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid user.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetDashboard_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new DashboardRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.GetDashboard(
                    It.IsAny<DashboardRequestDto>(),
                    It.IsAny<List<string>>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<DashboardModel>
                {
                    IsSuccess = true,
                    Result = new DashboardModel()
                });

            // Act
            var result = await _bal.GetDashboard(request, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task GetDashboard_Should_Return_Error_When_DAL_Throws_Exception()
        {
            // Arrange
            var request = new DashboardRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.GetDashboard(
                    It.IsAny<DashboardRequestDto>(),
                    It.IsAny<List<string>>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _bal.GetDashboard(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("DB error", result.ErrorMsgs);
        }
    }
}