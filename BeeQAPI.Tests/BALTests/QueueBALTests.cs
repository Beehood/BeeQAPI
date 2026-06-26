using BAL.Services;
using DAL.ContractIF;
using Models;
using Moq;
using System.Data;
using Xunit;

namespace BeeQAPI.Tests.BALTests
{
    public class QueueBALTests
    {
        private readonly Mock<IDAL_Queue> _mockDal;
        private readonly BAL_Queue _bal;

        public QueueBALTests()
        {
            _mockDal = new Mock<IDAL_Queue>();
            _bal = new BAL_Queue(_mockDal.Object);
        }

        // ========================
        // GET ALL
        // ========================

        [Fact]
        public async Task GetAll_Should_Return_AccessDenied_When_User_Has_No_Valid_Role()
        {
            var request = new PaginationRequestDto();
            var roles = new List<string> { "User" };
            var email = "test@test.com";

            var result = await _bal.GetAll(request, roles, email);

            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetAll_Should_Return_Data_When_User_Has_Valid_Role()
        {
            var request = new PaginationRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.GetAll(
                    It.IsAny<PaginationRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<List<QueueModel>>
                {
                    IsSuccess = true,
                    Result = new List<QueueModel>
                    {
                        new QueueModel()
                    }
                });

            var result = await _bal.GetAll(request, roles, email);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        // ========================
        // GET BY ID
        // ========================

        [Fact]
        public async Task GetById_Should_Return_AccessDenied_When_User_Has_No_Valid_Role()
        {
            var roles = new List<string> { "User" };
            var email = "test@test.com";

            var result = await _bal.GetById(1, roles, email);

            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetById_Should_Return_Data_When_User_Has_Valid_Role()
        {
            var roles = new List<string> { "Org Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.GetById(
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<QueueModel>
                {
                    IsSuccess = true,
                    Result = new QueueModel()
                });

            var result = await _bal.GetById(1, roles, email);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        // ========================
        // CREATE
        // ========================

        [Fact]
        public async Task Create_Should_Return_Error_When_User_Has_No_Valid_Role()
        {
            var request = new QueueRequestDto();
            var roles = new List<string> { "User" };
            var email = "test@test.com";

            var result = await _bal.Create(request, roles, email);

            Assert.False(result.IsSuccess);
            Assert.Contains("Only Admin can create token.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_Error_When_Request_Is_Null()
        {
            QueueRequestDto request = null;
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            var result = await _bal.Create(request, roles, email);

            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid payload.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_ValidationErrors_When_RequiredFields_Are_Missing()
        {
            var request = new QueueRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            var result = await _bal.Create(request, roles, email);

            Assert.False(result.IsSuccess);
            Assert.Contains("Branch is required", result.ErrorMsgs);
            Assert.Contains("Service is required", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
        {
            var request = new QueueRequestDto
            {
                BranchId = 1,
                BranchServiceId = 2
            };

            var roles = new List<string> { "Branch Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.Insert(
                    It.IsAny<QueueRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<int>
                {
                    IsSuccess = true,
                    Result = 1
                });

            var result = await _bal.Create(request, roles, email);

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }

        // ========================
        // UPDATE
        // ========================

        [Fact]
        public async Task Update_Should_Return_Error_When_User_Has_No_Valid_Role()
        {
            var request = new QueueRequestDto();
            var roles = new List<string> { "User" };
            var email = "test@test.com";

            var result = await _bal.Update(request, roles, email);

            Assert.False(result.IsSuccess);
            Assert.Contains("Only Admin can update queue.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_Error_When_Request_Is_Invalid()
        {
            var request = new QueueRequestDto
            {
                TokenId = 0
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            var result = await _bal.Update(request, roles, email);

            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid token data.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_Success_When_Request_Is_Valid()
        {
            var request = new QueueRequestDto
            {
                TokenId = 1
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.Update(
                    It.IsAny<QueueRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<int>
                {
                    IsSuccess = true,
                    Result = 1
                });

            var result = await _bal.Update(request, roles, email);

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }

        // ========================
        // CHANGE STATUS
        // ========================

        [Fact]
        public async Task ChangeStatus_Should_Return_Error_When_User_Has_No_Valid_Role()
        {
            var request = new QueueRequestDto { TokenId = 1 };
            var roles = new List<string> { "User" };
            var email = "test@test.com";

            var result = await _bal.ChangeStatus(request, roles, email);

            Assert.False(result.IsSuccess);
            Assert.Contains("Unauthorized access.", result.ErrorMsgs);
        }

        [Fact]
        public async Task ChangeStatus_Should_Return_Error_When_TokenId_Is_Invalid()
        {
            var request = new QueueRequestDto { TokenId = 0 };
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            var result = await _bal.ChangeStatus(request, roles, email);

            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid token ID.", result.ErrorMsgs);
        }

        [Fact]
        public async Task ChangeStatus_Should_Return_Success_When_Request_Is_Valid()
        {
            var request = new QueueRequestDto { TokenId = 1 };
            var roles = new List<string> { "Org Admin" };
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.ChangeStatus(
                    It.IsAny<QueueRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<int>
                {
                    IsSuccess = true,
                    Result = 1
                });

            var result = await _bal.ChangeStatus(request, roles, email);

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }

        // ========================
        // QUEUE DISPLAY
        // ========================

        [Fact]
        public async Task GetQueueDisplay_Should_Return_Data_When_DAL_Returns_Success()
        {
            var branchId = "1";

            _mockDal
                .Setup(x => x.GetQueueDisplay(It.IsAny<string>()))
                .ReturnsAsync(new APIGetResponseModel<List<QueueDisplayModel>>
                {
                    IsSuccess = true,
                    Result = new List<QueueDisplayModel>
                    {
                        new QueueDisplayModel()
                    }
                });

            var result = await _bal.GetQueueDisplay(branchId);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        // ========================
        // DROPDOWN
        // ========================

        [Fact]
        public async Task GetDropdown_Should_Return_Data_When_DAL_Returns_Success()
        {
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

            var result = await _bal.GetDropdown(email);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }
    }
}