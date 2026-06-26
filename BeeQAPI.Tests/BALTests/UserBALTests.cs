using BAL.Services;
using DAL.ContractIF;
using Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace BeeQAPI.Tests.BALTests
{
    public class UserBALTests
    {
        private readonly Mock<IDAL_User> _mockDal;
        private readonly BAL_User _bal;

        public UserBALTests()
        {
            _mockDal = new Mock<IDAL_User>();
            _bal = new BAL_User(_mockDal.Object);
        }

        // ========================
        // GET ALL
        // ========================

        [Fact]
        public async Task GetAll_Should_Return_AccessDenied_When_Role_Is_Invalid()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var roles = new List<string> { "Counter Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.GetAll(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetAll_Should_Return_Data_When_Role_Is_Valid()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            var expected = new APIGetResponseModel<List<UserModel>>
            {
                IsSuccess = true,
                Result = new List<UserModel>
                {
                    new UserModel { UserId = 1, Name = "Test User" }
                }
            };

            _mockDal.Setup(x => x.GetAll(request, email, null))
                    .ReturnsAsync(expected);

            // Act
            var result = await _bal.GetAll(request, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.Single(result.Result);
        }

        // ========================
        // GET BY ID
        // ========================

        [Fact]
        public async Task GetById_Should_Return_AccessDenied_When_Role_Is_Invalid()
        {
            // Arrange
            long id = 1;
            var roles = new List<string> { "Counter Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.GetById(id, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetById_Should_Return_Data_When_Role_Is_Valid()
        {
            // Arrange
            long id = 1;
            var roles = new List<string> { "Org Admin" };
            var email = "test@test.com";

            var expected = new APIGetResponseModel<UserModel>
            {
                IsSuccess = true,
                Result = new UserModel
                {
                    UserId = 1,
                    Name = "Swapna"
                }
            };

            _mockDal.Setup(x => x.GetById(id, email, null))
                    .ReturnsAsync(expected);

            // Act
            var result = await _bal.GetById(id, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.Equal("Swapna", result.Result.Name);
        }

        // ========================
        // CREATE
        // ========================

        [Fact]
        public async Task Create_Should_Return_AccessDenied_When_Role_Is_Invalid()
        {
            // Arrange
            var request = new UserRequestDto();
            var roles = new List<string> { "Branch Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Create(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_Error_When_Request_Is_Null()
        {
            // Arrange
            UserRequestDto request = null;
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Create(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid payload.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_ValidationErrors_When_Required_Fields_Are_Missing()
        {
            // Arrange
            var request = new UserRequestDto
            {
                Name = "",
                Email = "",
                Password = "",
                RoleId = 0
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Create(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("User Name is required", result.ErrorMsgs);
            Assert.Contains("Email is required", result.ErrorMsgs);
            Assert.Contains("Password is required", result.ErrorMsgs);
            Assert.Contains("Role is required", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new UserRequestDto
            {
                Name = "Swapna",
                Email = "swapna@test.com",
                Password = "123456",
                RoleId = 1
            };

            var roles = new List<string> { "Super Admin" };
            var email = "admin@test.com";

            var expected = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _mockDal.Setup(x => x.Insert(It.IsAny<UserRequestDto>(), email, null))
                    .ReturnsAsync(expected);

            // Act
            var result = await _bal.Create(request, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }

        [Fact]
        public async Task Create_Should_Return_Error_When_DAL_Throws_Exception()
        {
            // Arrange
            var request = new UserRequestDto
            {
                Name = "Swapna",
                Email = "swapna@test.com",
                Password = "123456",
                RoleId = 1
            };

            var roles = new List<string> { "Super Admin" };
            var email = "admin@test.com";

            _mockDal.Setup(x => x.Insert(It.IsAny<UserRequestDto>(), email, null))
                    .ThrowsAsync(new Exception("DB Error"));

            // Act
            var result = await _bal.Create(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("DB Error", result.ErrorMsgs);
        }

        // ========================
        // UPDATE
        // ========================

        [Fact]
        public async Task Update_Should_Return_AccessDenied_When_Role_Is_Invalid()
        {
            // Arrange
            var request = new UserRequestDto();
            var roles = new List<string> { "Branch Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Update(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_Error_When_Request_Is_Invalid()
        {
            // Arrange
            var request = new UserRequestDto
            {
                UserId = 0
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Update(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid user data.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_ValidationErrors_When_Required_Fields_Are_Missing()
        {
            // Arrange
            var request = new UserRequestDto
            {
                UserId = 1,
                Name = "",
                Email = "",
                RoleId = 0
            };

            var roles = new List<string> { "Org Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Update(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("User Name is required", result.ErrorMsgs);
            Assert.Contains("Email is required", result.ErrorMsgs);
            Assert.Contains("Role is required", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new UserRequestDto
            {
                UserId = 1,
                Name = "Updated User",
                Email = "updated@test.com",
                Password = "newpass",
                RoleId = 2
            };

            var roles = new List<string> { "Super Admin" };
            var email = "admin@test.com";

            var expected = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _mockDal.Setup(x => x.Update(It.IsAny<UserRequestDto>(), email, null))
                    .ReturnsAsync(expected);

            // Act
            var result = await _bal.Update(request, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }

        [Fact]
        public async Task Update_Should_Return_Error_When_DAL_Throws_Exception()
        {
            // Arrange
            var request = new UserRequestDto
            {
                UserId = 1,
                Name = "Updated User",
                Email = "updated@test.com",
                Password = "newpass",
                RoleId = 2
            };

            var roles = new List<string> { "Super Admin" };
            var email = "admin@test.com";

            _mockDal.Setup(x => x.Update(It.IsAny<UserRequestDto>(), email, null))
                    .ThrowsAsync(new Exception("DB Error"));

            // Act
            var result = await _bal.Update(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("DB Error", result.ErrorMsgs);
        }

        // ========================
        // CHANGE STATUS
        // ========================

        [Fact]
        public async Task ChangeStatus_Should_Return_AccessDenied_When_Role_Is_Invalid()
        {
            // Arrange
            long id = 1;
            var roles = new List<string> { "Branch Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.ChangeStatus(id, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task ChangeStatus_Should_Return_Error_When_Id_Is_Invalid()
        {
            // Arrange
            long id = 0;
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.ChangeStatus(id, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid user ID.", result.ErrorMsgs);
        }

        [Fact]
        public async Task ChangeStatus_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            long id = 1;
            var roles = new List<string> { "Org Admin" };
            var email = "test@test.com";

            var expected = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _mockDal.Setup(x => x.ChangeStatus(id, email, null))
                    .ReturnsAsync(expected);

            // Act
            var result = await _bal.ChangeStatus(id, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }

        [Fact]
        public async Task ChangeStatus_Should_Return_Error_When_DAL_Throws_Exception()
        {
            // Arrange
            long id = 1;
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal.Setup(x => x.ChangeStatus(id, email, null))
                    .ThrowsAsync(new Exception("DB Error"));

            // Act
            var result = await _bal.ChangeStatus(id, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("DB Error", result.ErrorMsgs);
        }

        // ========================
        // DROPDOWN
        // ========================

        [Fact]
        public async Task GetDropdown_Should_Return_Data_When_DAL_Returns_Data()
        {
            // Arrange
            var email = "test@test.com";

            var expected = new APIGetResponseModel<List<DropdownModel>>
            {
                IsSuccess = true,
                Result = new List<DropdownModel>
                {
                    new DropdownModel { Id = 1, Name = "User 1" }
                }
            };

            _mockDal.Setup(x => x.GetDropdown(email, null))
                    .ReturnsAsync(expected);

            // Act
            var result = await _bal.GetDropdown(email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.Single(result.Result);
        }

        [Fact]
        public async Task GetDropdown_Should_Return_Error_When_DAL_Throws_Exception()
        {
            // Arrange
            var email = "test@test.com";

            _mockDal.Setup(x => x.GetDropdown(email, null))
                    .ThrowsAsync(new Exception("DB Error"));

            // Act
            var result = await _bal.GetDropdown(email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("DB Error", result.ErrorMsgs);
        }
    }
}