//using BAL.Services;
//using DAL.ContractIF;
//using Models;
//using Moq;
//using System.Data;
//using Xunit;

//namespace BeeQAPI.Tests.BALTests
//{
//    public class RoleBALTests
//    {
//        private readonly Mock<IDAL_Role> _mockDal;
//        private readonly BAL_Role _bal;

//        public RoleBALTests()
//        {
//            _mockDal = new Mock<IDAL_Role>();
//            _bal = new BAL_Role(_mockDal.Object);
//        }

//        // ========================
//        // GET ALL
//        // ========================

//        [Fact]
//        public async Task GetAll_Should_Return_AccessDenied_When_User_Has_No_Valid_Role()
//        {
//            // Arrange
//            var request = new PaginationRequestDto();
//            var roles = new List<string> { "User" };
//            var email = "test@test.com";

//            // Act
//            var result = await _bal.GetAll(request, roles, email);

//            // Assert
//            Assert.False(result.IsSuccess);
//            Assert.Contains("Access denied.", result.ErrorMsgs);
//        }

//        [Fact]
//        public async Task GetAll_Should_Return_Data_When_User_Has_SuperAdmin_Role()
//        {
//            // Arrange
//            var request = new PaginationRequestDto();
//            var roles = new List<string> { "Super Admin" };
//            var email = "test@test.com";

//            _mockDal
//                .Setup(x => x.GetAll(
//                    It.IsAny<PaginationRequestDto>(),
//                    It.IsAny<string>(),
//                    It.IsAny<IDbTransaction>()))
//                .ReturnsAsync(new APIGetResponseModel<List<RoleModel>>
//                {
//                    IsSuccess = true,
//                    Result = new List<RoleModel> { new RoleModel() }
//                });

//            // Act
//            var result = await _bal.GetAll(request, roles, email);

//            // Assert
//            Assert.True(result.IsSuccess);
//            Assert.NotNull(result.Result);
//        }

//        // ========================
//        // GET BY ID
//        // ========================

//        [Fact]
//        public async Task GetById_Should_Return_AccessDenied_When_User_Has_No_Valid_Role()
//        {
//            // Arrange
//            long id = 1;
//            var roles = new List<string> { "User" };
//            var email = "test@test.com";

//            // Act
//            var result = await _bal.GetById(id, roles, email);

//            // Assert
//            Assert.False(result.IsSuccess);
//            Assert.Contains("Access denied.", result.ErrorMsgs);
//        }

//        [Fact]
//        public async Task GetById_Should_Return_Data_When_User_Has_OrgAdmin_Role()
//        {
//            // Arrange
//            long id = 1;
//            var roles = new List<string> { "Org Admin" };
//            var email = "test@test.com";

//            _mockDal
//                .Setup(x => x.GetById(
//                    It.IsAny<long>(),
//                    It.IsAny<string>(),
//                    It.IsAny<IDbTransaction>()))
//                .ReturnsAsync(new APIGetResponseModel<RoleModel>
//                {
//                    IsSuccess = true,
//                    Result = new RoleModel()
//                });

//            // Act
//            var result = await _bal.GetById(id, roles, email);

//            // Assert
//            Assert.True(result.IsSuccess);
//            Assert.NotNull(result.Result);
//        }

//        // ========================
//        // CREATE
//        // ========================

//        [Fact]
//        public async Task Create_Should_Return_AccessDenied_When_User_Is_Not_SuperAdmin()
//        {
//            // Arrange
//            var request = new RoleRequestDto
//            {
//                RoleName = "Admin",
//                RoleCode = "ADMIN"
//            };
//            var roles = new List<string> { "Org Admin" };
//            var email = "test@test.com";

//            // Act
//            var result = await _bal.Create(request, roles, email);

//            // Assert
//            Assert.False(result.IsSuccess);
//            Assert.Contains("Access denied.", result.ErrorMsgs);
//        }

//        [Fact]
//        public async Task Create_Should_Return_Error_When_Request_Is_Null()
//        {
//            // Arrange
//            RoleRequestDto request = null;
//            var roles = new List<string> { "Super Admin" };
//            var email = "test@test.com";

//            // Act
//            var result = await _bal.Create(request, roles, email);

//            // Assert
//            Assert.False(result.IsSuccess);
//            Assert.Contains("Invalid payload.", result.ErrorMsgs);
//        }

//        [Fact]
//        public async Task Create_Should_Return_ValidationErrors_When_RequiredFields_Are_Missing()
//        {
//            // Arrange
//            var request = new RoleRequestDto();
//            var roles = new List<string> { "Super Admin" };
//            var email = "test@test.com";

//            // Act
//            var result = await _bal.Create(request, roles, email);

//            // Assert
//            Assert.False(result.IsSuccess);
//            Assert.Contains("Role name is required", result.ErrorMsgs);
//            Assert.Contains("Role code is required", result.ErrorMsgs);
//        }

//        [Fact]
//        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
//        {
//            // Arrange
//            var request = new RoleRequestDto
//            {
//                RoleName = "Admin",
//                RoleCode = "ADMIN"
//            };
//            var roles = new List<string> { "Super Admin" };
//            var email = "test@test.com";

//            _mockDal
//                .Setup(x => x.Insert(
//                    It.IsAny<RoleRequestDto>(),
//                    It.IsAny<string>(),
//                    It.IsAny<IDbTransaction>()))
//                .ReturnsAsync(new APIGetResponseModel<int>
//                {
//                    IsSuccess = true,
//                    Result = 1
//                });

//            // Act
//            var result = await _bal.Create(request, roles, email);

//            // Assert
//            Assert.True(result.IsSuccess);
//            Assert.Equal(1, result.Result);
//        }

//        // ========================
//        // UPDATE
//        // ========================

//        [Fact]
//        public async Task Update_Should_Return_AccessDenied_When_User_Is_Not_SuperAdmin()
//        {
//            // Arrange
//            var request = new RoleRequestDto
//            {
//                RoleId = 1,
//                RoleName = "Admin",
//                RoleCode = "ADMIN"
//            };
//            var roles = new List<string> { "Org Admin" };
//            var email = "test@test.com";

//            // Act
//            var result = await _bal.Update(request, roles, email);

//            // Assert
//            Assert.False(result.IsSuccess);
//            Assert.Contains("Access denied.", result.ErrorMsgs);
//        }

//        [Fact]
//        public async Task Update_Should_Return_Error_When_RoleId_Is_Invalid()
//        {
//            // Arrange
//            var request = new RoleRequestDto
//            {
//                RoleId = 0,
//                RoleName = "Admin",
//                RoleCode = "ADMIN"
//            };
//            var roles = new List<string> { "Super Admin" };
//            var email = "test@test.com";

//            // Act
//            var result = await _bal.Update(request, roles, email);

//            // Assert
//            Assert.False(result.IsSuccess);
//            Assert.Contains("Invalid role data.", result.ErrorMsgs);
//        }

//        [Fact]
//        public async Task Update_Should_Return_ValidationErrors_When_RequiredFields_Are_Missing()
//        {
//            // Arrange
//            var request = new RoleRequestDto
//            {
//                RoleId = 1
//            };
//            var roles = new List<string> { "Super Admin" };
//            var email = "test@test.com";

//            // Act
//            var result = await _bal.Update(request, roles, email);

//            // Assert
//            Assert.False(result.IsSuccess);
//            Assert.Contains("Role name is required", result.ErrorMsgs);
//            Assert.Contains("Role code is required", result.ErrorMsgs);
//        }

//        [Fact]
//        public async Task Update_Should_Return_Success_When_Request_Is_Valid()
//        {
//            // Arrange
//            var request = new RoleRequestDto
//            {
//                RoleId = 1,
//                RoleName = "Updated Admin",
//                RoleCode = "UPDATED_ADMIN"
//            };
//            var roles = new List<string> { "Super Admin" };
//            var email = "test@test.com";

//            _mockDal
//                .Setup(x => x.Update(
//                    It.IsAny<RoleRequestDto>(),
//                    It.IsAny<string>(),
//                    It.IsAny<IDbTransaction>()))
//                .ReturnsAsync(new APIGetResponseModel<int>
//                {
//                    IsSuccess = true,
//                    Result = 1
//                });

//            // Act
//            var result = await _bal.Update(request, roles, email);

//            // Assert
//            Assert.True(result.IsSuccess);
//            Assert.Equal(1, result.Result);
//        }

//        // ========================
//        // CHANGE STATUS
//        // ========================

//        [Fact]
//        public async Task ChangeStatus_Should_Return_AccessDenied_When_User_Is_Not_SuperAdmin()
//        {
//            // Arrange
//            long id = 1;
//            var roles = new List<string> { "Org Admin" };
//            var email = "test@test.com";

//            // Act
//            var result = await _bal.ChangeStatus(id, roles, email);

//            // Assert
//            Assert.False(result.IsSuccess);
//            Assert.Contains("Access denied.", result.ErrorMsgs);
//        }

//        [Fact]
//        public async Task ChangeStatus_Should_Return_Error_When_Id_Is_Invalid()
//        {
//            // Arrange
//            long id = 0;
//            var roles = new List<string> { "Super Admin" };
//            var email = "test@test.com";

//            // Act
//            var result = await _bal.ChangeStatus(id, roles, email);

//            // Assert
//            Assert.False(result.IsSuccess);
//            Assert.Contains("Invalid role ID.", result.ErrorMsgs);
//        }

//        [Fact]
//        public async Task ChangeStatus_Should_Return_Success_When_Request_Is_Valid()
//        {
//            // Arrange
//            long id = 1;
//            var roles = new List<string> { "Super Admin" };
//            var email = "test@test.com";

//            _mockDal
//                .Setup(x => x.ChangeStatus(
//                    It.IsAny<long>(),
//                    It.IsAny<string>(),
//                    It.IsAny<IDbTransaction>()))
//                .ReturnsAsync(new APIGetResponseModel<int>
//                {
//                    IsSuccess = true,
//                    Result = 1
//                });

//            // Act
//            var result = await _bal.ChangeStatus(id, roles, email);

//            // Assert
//            Assert.True(result.IsSuccess);
//            Assert.Equal(1, result.Result);
//        }

//        // ========================
//        // DROPDOWN
//        // ========================

//        [Fact]
//        public async Task GetDropdown_Should_Return_Data_When_DAL_Returns_Data()
//        {
//            // Arrange
//            var email = "test@test.com";

//            _mockDal
//                .Setup(x => x.GetDropdown(
//                    It.IsAny<string>(),
//                    It.IsAny<IDbTransaction>()))
//                .ReturnsAsync(new APIGetResponseModel<List<DropdownModel>>
//                {
//                    IsSuccess = true,
//                    Result = new List<DropdownModel>
//                    {
//                        new DropdownModel()
//                    }
//                });

//            // Act
//            var result = await _bal.GetDropdown(email);

//            // Assert
//            Assert.True(result.IsSuccess);
//            Assert.NotNull(result.Result);
//        }

//        [Fact]
//        public async Task GetDropdown_Should_Return_Error_When_DAL_Throws_Exception()
//        {
//            // Arrange
//            var email = "test@test.com";

//            _mockDal
//                .Setup(x => x.GetDropdown(
//                    It.IsAny<string>(),
//                    It.IsAny<IDbTransaction>()))
//                .ThrowsAsync(new Exception("Dropdown error"));

//            // Act
//            var result = await _bal.GetDropdown(email);

//            // Assert
//            Assert.False(result.IsSuccess);
//            Assert.Contains("Dropdown error", result.ErrorMsgs);
//        }
//    }
//}