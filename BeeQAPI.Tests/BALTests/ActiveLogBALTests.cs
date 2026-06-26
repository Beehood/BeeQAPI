using Xunit;
using Moq;
using BAL.Services;
using DAL.ContractIF;
using Models;
using System.Data;

namespace BeeQAPI.Tests.BALTests
{
    public class ActiveLogBALTests
    {
        private readonly Mock<IDAL_ActiveLog> _mockDal;
        private readonly BAL_ActiveLog _bal;

        public ActiveLogBALTests()
        {
            _mockDal = new Mock<IDAL_ActiveLog>();
            _bal = new BAL_ActiveLog(_mockDal.Object);
        }

        // =========================
        // GET ALL
        // =========================
        [Fact]
        public async Task GetAll_Should_Return_AccessDenied_When_Role_Is_Invalid()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var roles = new List<string> { "User" };

            // Act
            var result = await _bal.GetAll(request, roles, "test@test.com");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetAll_Should_Return_Data_When_Role_Is_Valid()
        {
            // Arrange
            _mockDal.Setup(x => x.GetAll(
                    It.IsAny<PaginationRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<List<ActivityLogModel>>
                {
                    IsSuccess = true,
                    Result = new List<ActivityLogModel>()
                });

            // Act
            var result = await _bal.GetAll(
                new PaginationRequestDto(),
                new List<string> { "Super Admin" },
                "test@test.com");

            // Assert
            Assert.True(result.IsSuccess);
        }

        // =========================
        // GET BY ID
        // =========================
        [Fact]
        public async Task GetById_Should_Return_AccessDenied_When_Role_Is_Invalid()
        {
            // Arrange + Act
            var result = await _bal.GetById(
                1,
                new List<string> { "User" },
                "test@test.com");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetById_Should_Return_Data_When_Role_Is_Valid()
        {
            // Arrange
            _mockDal.Setup(x => x.GetById(
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<ActivityLogModel>
                {
                    IsSuccess = true,
                    Result = new ActivityLogModel()
                });

            // Act
            var result = await _bal.GetById(
                1,
                new List<string> { "Super Admin" },
                "test@test.com");

            // Assert
            Assert.True(result.IsSuccess);
        }

        // =========================
        // CREATE
        // =========================
        [Fact]
        public async Task Create_Should_Return_Error_When_Request_Is_Null()
        {
            // Arrange
            ActivityLogRequestDto request = null;

            // Act
            var result = await _bal.Create(
                request,
                new List<string> { "Super Admin" },
                "test@test.com");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid payload.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new ActivityLogRequestDto();

            _mockDal.Setup(x => x.Insert(
                    It.IsAny<ActivityLogRequestDto>(),
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
                new List<string> { "Super Admin" },
                "test@test.com");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }
    }
}