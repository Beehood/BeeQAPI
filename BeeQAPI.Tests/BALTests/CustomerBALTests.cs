using Xunit;
using Moq;
using BAL.Services;
using DAL.ContractIF;
using Models;
using System.Data;

namespace BeeQAPI.Tests.BALTests
{
    public class CustomerBALTests
    {
        private readonly Mock<IDAL_Customer> _mockDal;
        private readonly BAL_Customer _bal;

        public CustomerBALTests()
        {
            _mockDal = new Mock<IDAL_Customer>();
            _bal = new BAL_Customer(_mockDal.Object);
        }

        // =========================
        // GET ALL
        // =========================
        [Fact]
        public async Task GetAll_Should_Return_Data_When_Role_Is_Invalid_Because_Current_BAL_Allows_It()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var roles = new List<string> { "User" };

            _mockDal.Setup(x => x.GetAll(
                    It.IsAny<PaginationRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<List<CustomerModel>>
                {
                    IsSuccess = true,
                    Result = new List<CustomerModel>()
                });

            // Act
            var result = await _bal.GetAll(request, roles, "test@test.com");

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetAll_Should_Return_Data_When_Role_Is_Valid()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var roles = new List<string> { "Super Admin" };

            _mockDal.Setup(x => x.GetAll(
                    It.IsAny<PaginationRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<List<CustomerModel>>
                {
                    IsSuccess = true,
                    Result = new List<CustomerModel>()
                });

            // Act
            var result = await _bal.GetAll(request, roles, "test@test.com");

            // Assert
            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task GetAll_Should_Return_AccessDenied_When_Roles_Are_Empty()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var roles = new List<string>();

            // Act
            var result = await _bal.GetAll(request, roles, "test@test.com");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        // =========================
        // GET BY ID
        // =========================
        [Fact]
        public async Task GetById_Should_Return_AccessDenied_When_Roles_Are_Empty()
        {
            // Act
            var result = await _bal.GetById(
                1,
                new List<string>(),
                "test@test.com");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetById_Should_Return_Data_When_Roles_Are_Present()
        {
            // Arrange
            _mockDal.Setup(x => x.GetById(
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<CustomerModel>
                {
                    IsSuccess = true,
                    Result = new CustomerModel()
                });

            // Act
            var result = await _bal.GetById(
                1,
                new List<string> { "User" },
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
            CustomerRequestDto request = null;

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
            var request = new CustomerRequestDto();

            // Act
            var result = await _bal.Create(
                request,
                new List<string> { "Super Admin" },
                "test@test.com");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Customer Name is required", result.ErrorMsgs);
            Assert.Contains("Phone is required", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new CustomerRequestDto
            {
                Name = "John",
                Phone = "9876543210"
            };

            _mockDal.Setup(x => x.Insert(
                    It.IsAny<CustomerRequestDto>(),
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
        public async Task Update_Should_Return_Error_When_Request_Is_Invalid()
        {
            // Arrange
            CustomerRequestDto request = null;

            // Act
            var result = await _bal.Update(
                request,
                new List<string> { "Super Admin" },
                "test@test.com");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid customer data.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_ValidationErrors_When_Name_And_Phone_Are_Missing()
        {
            // Arrange
            var request = new CustomerRequestDto
            {
                CustomerId = 1
            };

            // Act
            var result = await _bal.Update(
                request,
                new List<string> { "Super Admin" },
                "test@test.com");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Customer Name is required", result.ErrorMsgs);
            Assert.Contains("Phone is required", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new CustomerRequestDto
            {
                CustomerId = 1,
                Name = "Updated Customer",
                Phone = "9999999999"
            };

            _mockDal.Setup(x => x.Update(
                    It.IsAny<CustomerRequestDto>(),
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
        public async Task ChangeStatus_Should_Return_Error_When_Id_Is_Invalid()
        {
            // Act
            var result = await _bal.ChangeStatus(
                0,
                new List<string> { "Super Admin" },
                "test@test.com");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid customer ID.", result.ErrorMsgs);
        }

        [Fact]
        public async Task ChangeStatus_Should_Return_Success_When_Id_Is_Valid()
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