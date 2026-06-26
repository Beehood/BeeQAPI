using BAL.Services;
using DAL.ContractIF;
using Models;
using Moq;
using System.Data;
using Xunit;

namespace BeeQAPI.Tests.BALTests
{
    public class ReportBALTests
    {
        private readonly Mock<IDAL_Report> _mockDal;
        private readonly BAL_Report _bal;

        public ReportBALTests()
        {
            _mockDal = new Mock<IDAL_Report>();
            _bal = new BAL_Report(_mockDal.Object);
        }

        // ========================
        // GET ALL
        // ========================

        [Fact]
        public async Task GetAll_Should_Return_AccessDenied_When_User_Has_No_Valid_Role()
        {
            // Arrange
            var request = new ReportRequestDto();
            var roles = new List<string> { "User" };
            var email = "test@test.com";

            // Act
            var result = await _bal.GetAll(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetAll_Should_Return_Data_When_User_Has_Valid_Role()
        {
            // Arrange
            var request = new ReportRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.GetAll(
                    It.IsAny<ReportRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<List<ReportModel>>
                {
                    IsSuccess = true,
                    Result = new List<ReportModel>
                    {
                        new ReportModel()
                    }
                });

            // Act
            var result = await _bal.GetAll(request, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task GetAll_Should_Return_Error_When_DAL_Throws_Exception()
        {
            // Arrange
            var request = new ReportRequestDto();
            var roles = new List<string> { "Branch Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.GetAll(
                    It.IsAny<ReportRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _bal.GetAll(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("DB error", result.ErrorMsgs);
        }
    }
}