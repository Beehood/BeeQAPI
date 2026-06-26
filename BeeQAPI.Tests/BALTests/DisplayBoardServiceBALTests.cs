using BAL.Services;
using DAL.ContractIF;
using Models;
using Moq;
using System.Data;
using Xunit;

namespace BeeQAPI.Tests.BALTests
{
    public class DisplayBoardServiceBALTests
    {
        private readonly Mock<IDAL_DisplayBoardService> _mockDal;
        private readonly BAL_DisplayBoardService _bal;

        public DisplayBoardServiceBALTests()
        {
            _mockDal = new Mock<IDAL_DisplayBoardService>();
            _bal = new BAL_DisplayBoardService(_mockDal.Object);
        }

        // ========================
        // GET ALL
        // ========================

        [Fact]
        public async Task GetAll_Should_Return_AccessDenied_When_UserHasNoValidRole()
        {
            // Arrange
            long displayId = 1;
            var roles = new List<string> { "User" };
            var email = "test@test.com";

            // Act
            var result = await _bal.GetAll(displayId, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetAll_Should_Return_Error_When_DisplayId_Is_Invalid()
        {
            // Arrange
            long displayId = -1;
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.GetAll(displayId, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid Display Id.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetAll_Should_Return_Data_When_Request_Is_Valid()
        {
            // Arrange
            long displayId = 1;
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.GetAll(
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<List<DisplayBoardServiceModel>>
                {
                    IsSuccess = true,
                    Result = new List<DisplayBoardServiceModel>
                    {
                        new DisplayBoardServiceModel()
                    }
                });

            // Act
            var result = await _bal.GetAll(displayId, roles, email);

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
            var request = new DisplayBoardServiceRequestDto();
            var roles = new List<string> { "Org Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Create(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Only Super Admin can map services.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_Error_When_Request_Is_Null()
        {
            // Arrange
            DisplayBoardServiceRequestDto request = null;
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
            var request = new DisplayBoardServiceRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Create(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Display Id is required", result.ErrorMsgs);
            Assert.Contains("Branch Service Id is required", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new DisplayBoardServiceRequestDto
            {
                DisplayId = 1,
                BranchServiceId = 2
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.Insert(
                    It.IsAny<DisplayBoardServiceRequestDto>(),
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
        // DELETE
        // ========================

        [Fact]
        public async Task Delete_Should_Return_Error_When_User_Is_Not_SuperAdmin()
        {
            // Arrange
            var roles = new List<string> { "Branch Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Delete(1, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Only Super Admin can delete mapping.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Delete_Should_Return_Error_When_Id_Is_Invalid()
        {
            // Arrange
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Delete(0, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid mapping Id.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Delete_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.Delete(
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<int>
                {
                    IsSuccess = true,
                    Result = 1
                });

            // Act
            var result = await _bal.Delete(1, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }
    }
}