using BAL.Services;
using DAL.ContractIF;
using Models;
using Moq;
using System.Data;
using Xunit;

namespace BeeQAPI.Tests
{
    public class AppointmentBALTests
    {
        private readonly Mock<IDAL_Appointment> _mockDal;
        private readonly BAL_Appointment _bal;

        public AppointmentBALTests()
        {
            _mockDal = new Mock<IDAL_Appointment>();
            _bal = new BAL_Appointment(_mockDal.Object);
        }

        [Fact]
        public async Task Create_Should_Return_AccessDenied_When_UserHasNoRole()
        {
            // Arrange
            var request = new AppointmentRequestDto();

            var roles = new List<string>
            {
                "User"
            };

            // Act
            var result = await _bal.Create(
                request,
                roles,
                "test@test.com");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }
        [Fact]
        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new AppointmentRequestDto
            {
                OrganizationId = 1,
                BranchId = 1,
                ServiceId = 1,
                CustomerName = "John",
                CustomerPhone = "9876543210",
                AppointmentDate = DateTime.Now
            };

            var roles = new List<string>
    {
        "Super Admin"
    };

            _mockDal
                .Setup(x => x.Insert(
                    It.IsAny<AppointmentRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<int>
                {
                    IsSuccess = true,
                    Result = 1
                });

            // Act
            var result = await _bal.Create(
                request,
                roles,
                "test@test.com");

            // Assert
            Assert.True(result.IsSuccess);
        }
        [Fact]
        public async Task Create_Should_Return_Error_When_Request_Is_Null()
        {
            // Arrange
            AppointmentRequestDto request = null;

            var roles = new List<string>
    {
        "Super Admin"
    };

            // Act
            var result = await _bal.Create(
                request,
                roles,
                "test@test.com");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid payload.", result.ErrorMsgs);
        }
        [Fact]
        public async Task Create_Should_Return_ValidationErrors_When_RequiredFields_Are_Missing()
        {
            var request = new AppointmentRequestDto();

            var roles = new List<string>
    {
        "Super Admin"
    };

            var result = await _bal.Create(
                request,
                roles,
                "test@test.com");

            Assert.False(result.IsSuccess);

            Assert.Contains("Customer name is required", result.ErrorMsgs);
            Assert.Contains("Customer phone is required", result.ErrorMsgs);
            Assert.Contains("Appointment date is required", result.ErrorMsgs);
        }
    }
}
