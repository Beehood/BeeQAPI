using BAL.Services;
using DAL.ContractIF;
using Models;
using Moq;
using System.Data;
using Xunit;

namespace BeeQAPI.Tests.BALTests
{
    public class PermissionBALTests
    {
        private readonly Mock<IDAL_Permission> _mockDal;
        private readonly BAL_Permission _bal;

        public PermissionBALTests()
        {
            _mockDal = new Mock<IDAL_Permission>();
            _bal = new BAL_Permission(_mockDal.Object);
        }

        // ========================
        // GET ALL
        // ========================

        [Fact]
        public async Task GetAll_Should_Return_AccessDenied_When_User_Has_No_Valid_Role()
        {
            // Arrange
            var request = new PaginationRequestDto();
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
            var request = new PaginationRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.GetAll(
                    It.IsAny<PaginationRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<List<PermissionModel>>
                {
                    IsSuccess = true,
                    Result = new List<PermissionModel>
                    {
                        new PermissionModel()
                    }
                });

            // Act
            var result = await _bal.GetAll(request, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        // ========================
        // GET BY ID
        // ========================

        [Fact]
        public async Task GetById_Should_Return_AccessDenied_When_User_Has_No_Valid_Role()
        {
            // Arrange
            var roles = new List<string> { "User" };
            var email = "test@test.com";

            // Act
            var result = await _bal.GetById(1, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetById_Should_Return_Data_When_User_Has_Valid_Role()
        {
            // Arrange
            var roles = new List<string> { "Org Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.GetById(
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<PermissionModel>
                {
                    IsSuccess = true,
                    Result = new PermissionModel()
                });

            // Act
            var result = await _bal.GetById(1, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        // ========================
        // CREATE
        // ========================

        [Fact]
        public async Task Create_Should_Return_Error_When_User_Is_Not_SuperAdmin()
        {
            // Arrange
            var request = new PermissionRequestDto();
            var roles = new List<string> { "Org Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Create(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Only Super Admin can create permissions.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_Error_When_Request_Is_Null()
        {
            // Arrange
            PermissionRequestDto request = null;
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Create(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid payload.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_ValidationErrors_When_RequiredFields_Are_Missing()
        {
            // Arrange
            var request = new PermissionRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Create(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Permission Name is required", result.ErrorMsgs);
            Assert.Contains("Permission Code is required", result.ErrorMsgs);
            Assert.Contains("Module is required", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new PermissionRequestDto
            {
                PermissionName = "View User",
                PermissionCode = "VIEW_USER",
                Module = "User"
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.Insert(
                    It.IsAny<PermissionRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<int>
                {
                    IsSuccess = true,
                    Result = 1
                });

            // Act
            var result = await _bal.Create(request, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }

        // ========================
        // UPDATE
        // ========================

        [Fact]
        public async Task Update_Should_Return_Error_When_User_Is_Not_SuperAdmin()
        {
            // Arrange
            var request = new PermissionRequestDto();
            var roles = new List<string> { "Branch Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Update(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Only Super Admin can update permissions.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_Error_When_Request_Is_Invalid()
        {
            // Arrange
            var request = new PermissionRequestDto
            {
                PermissionId = 0
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Update(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid permission data.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_ValidationErrors_When_RequiredFields_Are_Missing()
        {
            // Arrange
            var request = new PermissionRequestDto
            {
                PermissionId = 1
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Update(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Permission Name is required", result.ErrorMsgs);
            Assert.Contains("Permission Code is required", result.ErrorMsgs);
            Assert.Contains("Module is required", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new PermissionRequestDto
            {
                PermissionId = 1,
                PermissionName = "Edit User",
                PermissionCode = "EDIT_USER",
                Module = "User"
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.Update(
                    It.IsAny<PermissionRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<int>
                {
                    IsSuccess = true,
                    Result = 1
                });

            // Act
            var result = await _bal.Update(request, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }

        // ========================
        // CHANGE STATUS
        // ========================

        [Fact]
        public async Task ChangeStatus_Should_Return_Error_When_User_Is_Not_SuperAdmin()
        {
            // Arrange
            var roles = new List<string> { "Org Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.ChangeStatus(1, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Only Super Admin can change permission status.", result.ErrorMsgs);
        }

        [Fact]
        public async Task ChangeStatus_Should_Return_Error_When_Id_Is_Invalid()
        {
            // Arrange
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.ChangeStatus(0, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid permission ID.", result.ErrorMsgs);
        }

        [Fact]
        public async Task ChangeStatus_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.ChangeStatus(
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<int>
                {
                    IsSuccess = true,
                    Result = 1
                });

            // Act
            var result = await _bal.ChangeStatus(1, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }

        // ========================
        // DROPDOWN
        // ========================

        [Fact]
        public async Task GetDropdown_Should_Return_Data_When_DAL_Returns_Success()
        {
            // Arrange
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.GetDropdown(
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<List<DropdownModel>>
                {
                    IsSuccess = true,
                    Result = new List<DropdownModel>
                    {
                        new DropdownModel()
                    }
                });

            // Act
            var result = await _bal.GetDropdown(email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }
    }
}