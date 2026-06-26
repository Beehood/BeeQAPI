using BAL.Services;
using DAL.ContractIF;
using Models;
using Moq;
using System.Data;
using Xunit;

namespace BeeQAPI.Tests.BALTests
{
    public class DisplayBoardBALTests
    {
        private readonly Mock<IDAL_DisplayBoard> _mockDal;
        private readonly BAL_DisplayBoard _bal;

        public DisplayBoardBALTests()
        {
            _mockDal = new Mock<IDAL_DisplayBoard>();
            _bal = new BAL_DisplayBoard(_mockDal.Object);
        }

        // ========================
        // GET ALL
        // ========================

        [Fact]
        public async Task GetAll_Should_Return_AccessDenied_When_UserHasNoValidRole()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var roles = new List<string> { "User" };
            var email = "test@test.com";

            // Act
            var result = await _bal.GetAll(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied. Only Super Admin allowed.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetAll_Should_Return_Data_When_Role_Is_Valid()
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
                .ReturnsAsync(new APIGetResponseModel<List<DisplayBoardModel>>
                {
                    IsSuccess = true,
                    Result = new List<DisplayBoardModel>
                    {
                        new DisplayBoardModel()
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
        public async Task GetById_Should_Return_AccessDenied_When_UserHasNoValidRole()
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
        public async Task GetById_Should_Return_Data_When_Role_Is_Valid()
        {
            // Arrange
            var roles = new List<string> { "Org Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.GetById(
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<DisplayBoardModel>
                {
                    IsSuccess = true,
                    Result = new DisplayBoardModel()
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
            var request = new DisplayBoardRequestDto();
            var roles = new List<string> { "Org Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Create(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Only Super Admin can create display board.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_Error_When_Request_Is_Null()
        {
            // Arrange
            DisplayBoardRequestDto request = null;
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
            var request = new DisplayBoardRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Create(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Display Name is required", result.ErrorMsgs);
            Assert.Contains("Screen Code is required", result.ErrorMsgs);
            Assert.Contains("Branch is required", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new DisplayBoardRequestDto
            {
                DisplayName = "Display 1",
                ScreenCode = "SCR001",
                BranchId = 1
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.Insert(
                    It.IsAny<DisplayBoardRequestDto>(),
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
            var request = new DisplayBoardRequestDto();
            var roles = new List<string> { "Org Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Update(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Only Super Admin can update display board.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_Error_When_Request_Is_Invalid()
        {
            // Arrange
            var request = new DisplayBoardRequestDto
            {
                DisplayId = 0
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Update(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid display board data.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_ValidationErrors_When_RequiredFields_Are_Missing()
        {
            // Arrange
            var request = new DisplayBoardRequestDto
            {
                DisplayId = 1
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Update(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Display Name is required", result.ErrorMsgs);
            Assert.Contains("Screen Code is required", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new DisplayBoardRequestDto
            {
                DisplayId = 1,
                DisplayName = "Updated Display",
                ScreenCode = "SCR002"
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.Update(
                    It.IsAny<DisplayBoardRequestDto>(),
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
            Assert.Contains("Invalid display board ID.", result.ErrorMsgs);
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

        // ========================
        // DISPLAY DATA
        // ========================

        [Fact]
        public async Task GetDisplayData_Should_Return_Data_When_DAL_Returns_Success()
        {
            // Arrange
            var username = "displayuser";

            _mockDal
                .Setup(x => x.GetDisplayData(It.IsAny<string>()))
                .ReturnsAsync(new APIGetResponseModel<List<QueueDisplayModel>>
                {
                    IsSuccess = true,
                    Result = new List<QueueDisplayModel>
                    {
                        new QueueDisplayModel()
                    }
                });

            // Act
            var result = await _bal.GetDisplayData(username);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }
    }
}