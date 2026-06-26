using Xunit;
using Moq;
using BAL.Services;
using DAL.ContractIF;
using Models;
using System.Data;

namespace BeeQAPI.Tests.BALTests
{
    public class CounterServiceBALTests
    {
        private readonly Mock<IDAL_CounterService> _mockDal;
        private readonly BAL_CounterService _bal;

        public CounterServiceBALTests()
        {
            _mockDal = new Mock<IDAL_CounterService>();
            _bal = new BAL_CounterService(_mockDal.Object);
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
                .ReturnsAsync(new APIGetResponseModel<List<CounterServiceModel>>
                {
                    IsSuccess = true,
                    Result = new List<CounterServiceModel>()
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
            // Act
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
                .ReturnsAsync(new APIGetResponseModel<CounterServiceModel>
                {
                    IsSuccess = true,
                    Result = new CounterServiceModel()
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
        public async Task Create_Should_Return_Error_When_Role_Is_Invalid()
        {
            // Arrange
            var request = new CounterServiceRequestDto();

            // Act
            var result = await _bal.Create(
                request,
                new List<string> { "User" },
                "test@test.com");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_Error_When_Request_Is_Null()
        {
            // Arrange
            CounterServiceRequestDto request = null;

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
        public async Task Create_Should_Return_ValidationErrors_When_Required_Fields_Are_Missing()
        {
            // Arrange
            var request = new CounterServiceRequestDto();

            // Act
            var result = await _bal.Create(
                request,
                new List<string> { "Super Admin" },
                "test@test.com");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Counter is required", result.ErrorMsgs);
            Assert.Contains("BranchService is required", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new CounterServiceRequestDto
            {
                CounterId = 1,
                BranchServiceId = 1
            };

            _mockDal.Setup(x => x.Insert(
                    It.IsAny<CounterServiceRequestDto>(),
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

        // =========================
        // UPDATE
        // =========================
        [Fact]
        public async Task Update_Should_Return_Error_When_Role_Is_Invalid()
        {
            // Arrange
            var request = new CounterServiceRequestDto();

            // Act
            var result = await _bal.Update(
                request,
                new List<string> { "User" },
                "test@test.com");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_Error_When_Request_Is_Invalid()
        {
            // Arrange
            CounterServiceRequestDto request = null;

            // Act
            var result = await _bal.Update(
                request,
                new List<string> { "Super Admin" },
                "test@test.com");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid counter service data.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new CounterServiceRequestDto
            {
                CounterServiceId = 1,
                CounterId = 1,
                BranchServiceId = 1
            };

            _mockDal.Setup(x => x.Update(
                    It.IsAny<CounterServiceRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<int>
                {
                    IsSuccess = true,
                    Result = 1
                });

            // Act
            var result = await _bal.Update(
                request,
                new List<string> { "Super Admin" },
                "test@test.com");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }

        // =========================
        // CHANGE STATUS
        // =========================
        [Fact]
        public async Task ChangeStatus_Should_Return_Error_When_Role_Is_Invalid()
        {
            // Act
            var result = await _bal.ChangeStatus(
                1,
                new List<string> { "User" },
                "test@test.com");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task ChangeStatus_Should_Return_Error_When_Id_Is_Invalid()
        {
            // Act
            var result = await _bal.ChangeStatus(
                0,
                new List<string> { "Super Admin" },
                "test@test.com");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid CounterService ID.", result.ErrorMsgs);
        }

        [Fact]
        public async Task ChangeStatus_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            _mockDal.Setup(x => x.ChangeStatus(
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<int>
                {
                    IsSuccess = true,
                    Result = 1
                });

            // Act
            var result = await _bal.ChangeStatus(
                1,
                new List<string> { "Super Admin" },
                "test@test.com");

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }

        // =========================
        // DROPDOWN
        // =========================
        [Fact]
        public async Task GetDropdown_Should_Return_Data()
        {
            // Arrange
            _mockDal.Setup(x => x.GetDropdown(
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<List<DropdownModel>>
                {
                    IsSuccess = true,
                    Result = new List<DropdownModel>()
                });

            // Act
            var result = await _bal.GetDropdown("test@test.com");

            // Assert
            Assert.True(result.IsSuccess);
        }
    }
}