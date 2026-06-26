using BAL.Services;
using DAL.ContractIF;
using Models;
using Moq;
using System.Data;
using Xunit;

namespace BeeQAPI.Tests.BALTests
{
    public class OrganizationBALTests
    {
        private readonly Mock<IDAL_Organization> _mockDal;
        private readonly BAL_Organization _bal;

        public OrganizationBALTests()
        {
            _mockDal = new Mock<IDAL_Organization>();
            _bal = new BAL_Organization(_mockDal.Object);
        }

        // ========================
        // GET ALL
        // ========================

        [Fact]
        public async Task GetAll_Should_Return_AccessDenied_When_User_Is_Not_SuperAdmin()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var roles = new List<string> { "Org Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.GetAll(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied. Only Super Admin allowed.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetAll_Should_Return_Data_When_User_Is_SuperAdmin()
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
                .ReturnsAsync(new APIGetResponseModel<List<OrganizationModel>>
                {
                    IsSuccess = true,
                    Result = new List<OrganizationModel>
                    {
                        new OrganizationModel()
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
        public async Task GetById_Should_Return_AccessDenied_When_User_Is_Not_SuperAdmin()
        {
            // Arrange
            var roles = new List<string> { "Branch Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.GetById(1, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetById_Should_Return_Data_When_User_Is_SuperAdmin()
        {
            // Arrange
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.GetById(
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<OrganizationModel>
                {
                    IsSuccess = true,
                    Result = new OrganizationModel()
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
            var request = new OrganizationRequestDto();
            var roles = new List<string> { "Org Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Create(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Only Super Admin can create organization.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_Error_When_Request_Is_Null()
        {
            // Arrange
            OrganizationRequestDto request = null;
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
            var request = new OrganizationRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Create(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Organization Name is required", result.ErrorMsgs);
            Assert.Contains("Email is required", result.ErrorMsgs);
            Assert.Contains("Phone is required", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new OrganizationRequestDto
            {
                Name = "Test Org",
                Email = "org@test.com",
                Phone = "9999999999"
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.Insert(
                    It.IsAny<OrganizationRequestDto>(),
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
            var request = new OrganizationRequestDto();
            var roles = new List<string> { "Org Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Update(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Only Super Admin can update organization.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_Error_When_Request_Is_Invalid()
        {
            // Arrange
            var request = new OrganizationRequestDto
            {
                OrganizationId = 0
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Update(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid organization data.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_ValidationErrors_When_RequiredFields_Are_Missing()
        {
            // Arrange
            var request = new OrganizationRequestDto
            {
                OrganizationId = 1
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Update(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Organization Name is required", result.ErrorMsgs);
            Assert.Contains("Email is required", result.ErrorMsgs);
            Assert.Contains("Phone is required", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new OrganizationRequestDto
            {
                OrganizationId = 1,
                Name = "Updated Org",
                Email = "updated@test.com",
                Phone = "8888888888"
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.Update(
                    It.IsAny<OrganizationRequestDto>(),
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
            Assert.Contains("Only Super Admin can change status.", result.ErrorMsgs);
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
            Assert.Contains("Invalid organization ID.", result.ErrorMsgs);
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