using BAL.Services;
using DAL.ContractIF;
using Models;
using Moq;
using System.Data;
using Xunit;

namespace BeeQAPI.Tests.BALTests
{
    public class RolePermissionBALTests
    {
        private readonly Mock<IDAL_RolePermission> _mockDal;
        private readonly BAL_RolePermission _bal;

        public RolePermissionBALTests()
        {
            _mockDal = new Mock<IDAL_RolePermission>();
            _bal = new BAL_RolePermission(_mockDal.Object);
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
                .ReturnsAsync(new APIGetResponseModel<List<RolePermissionModel>>
                {
                    IsSuccess = true,
                    Result = new List<RolePermissionModel>
                    {
                        new RolePermissionModel()
                    }
                });

            // Act
            var result = await _bal.GetAll(request, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        // ========================
        // GET BY ROLE ID
        // ========================

        [Fact]
        public async Task GetByRoleId_Should_Return_AccessDenied_When_User_Has_No_Valid_Role()
        {
            // Arrange
            long roleId = 1;
            var roles = new List<string> { "User" };
            var email = "test@test.com";

            // Act
            var result = await _bal.GetByRoleId(roleId, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetByRoleId_Should_Return_Data_When_User_Has_Valid_Role()
        {
            // Arrange
            long roleId = 1;
            var roles = new List<string> { "Org Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.GetByRoleId(
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<List<RolePermissionModel>>
                {
                    IsSuccess = true,
                    Result = new List<RolePermissionModel>
                    {
                        new RolePermissionModel()
                    }
                });

            // Act
            var result = await _bal.GetByRoleId(roleId, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        // ========================
        // CREATE
        // ========================

        [Fact]
        public async Task Create_Should_Return_AccessDenied_When_User_Is_Not_SuperAdmin()
        {
            // Arrange
            var request = new RolePermissionRequestDto
            {
                RoleId = 1,
                PermissionId = 1
            };
            var roles = new List<string> { "Org Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Create(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Only Super Admin can assign permissions.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_Error_When_Request_Is_Invalid()
        {
            // Arrange
            var request = new RolePermissionRequestDto
            {
                RoleId = 0,
                PermissionId = 0
            };
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Create(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid role permission data.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new RolePermissionRequestDto
            {
                RoleId = 1,
                PermissionId = 2
            };
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.Insert(
                    It.IsAny<RolePermissionRequestDto>(),
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
        // BULK ASSIGN
        // ========================

        [Fact]
        public async Task BulkAssign_Should_Return_AccessDenied_When_User_Is_Not_SuperAdmin()
        {
            // Arrange
            var request = new RolePermissionRequestDto
            {
                RoleId = 1,
                PermissionIds = "1,2,3"
            };
            var roles = new List<string> { "Branch Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.BulkAssign(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Only Super Admin can assign permissions.", result.ErrorMsgs);
        }

        [Fact]
        public async Task BulkAssign_Should_Return_Error_When_Request_Is_Invalid()
        {
            // Arrange
            var request = new RolePermissionRequestDto
            {
                RoleId = 0,
                PermissionIds = ""
            };
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.BulkAssign(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid bulk permission data.", result.ErrorMsgs);
        }

        [Fact]
        public async Task BulkAssign_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new RolePermissionRequestDto
            {
                RoleId = 1,
                PermissionIds = "1,2,3"
            };
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.BulkInsert(
                    It.IsAny<RolePermissionRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<int>
                {
                    IsSuccess = true,
                    Result = 1
                });

            // Act
            var result = await _bal.BulkAssign(request, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }

        // ========================
        // DELETE
        // ========================

        [Fact]
        public async Task Delete_Should_Return_AccessDenied_When_User_Is_Not_SuperAdmin()
        {
            // Arrange
            var request = new RolePermissionRequestDto
            {
                RoleId = 1,
                PermissionId = 1
            };
            var roles = new List<string> { "Org Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Delete(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Only Super Admin can remove permissions.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Delete_Should_Return_Error_When_Request_Is_Invalid()
        {
            // Arrange
            var request = new RolePermissionRequestDto
            {
                RoleId = 0,
                PermissionId = 0
            };
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Delete(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid role permission data.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Delete_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new RolePermissionRequestDto
            {
                RoleId = 1,
                PermissionId = 2
            };
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.Delete(
                    It.IsAny<RolePermissionRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<int>
                {
                    IsSuccess = true,
                    Result = 1
                });

            // Act
            var result = await _bal.Delete(request, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }
    }
}