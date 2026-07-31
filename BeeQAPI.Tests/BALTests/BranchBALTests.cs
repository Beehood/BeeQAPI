//using Xunit;
//using Moq;
//using BAL.Services;
//using DAL.ContractIF;
//using Models;
//using System.Data;

//namespace BeeQAPI.Tests.BALTests
//{
//    public class BranchBALTests
//    {
//        private readonly Mock<IDAL_Branch> _mockDal;
//        private readonly BAL_Branch _bal;

//        public BranchBALTests()
//        {
//            _mockDal = new Mock<IDAL_Branch>();
//            _bal = new BAL_Branch(_mockDal.Object);
//        }

//        // =========================
//        // GET ALL
//        // =========================
//        [Fact]
//        public async Task GetAll_Should_Return_AccessDenied_When_Role_Is_Invalid()
//        {
//            var request = new PaginationRequestDto();
//            var roles = new List<string> { "User" };

//            var result = await _bal.GetAll(request, roles, "test@test.com");

//            Assert.False(result.IsSuccess);
//            Assert.Contains("Access denied. Only Admins allowed.", result.ErrorMsgs);
//        }

//        [Fact]
//        public async Task GetAll_Should_Return_Data_When_Role_Is_Valid()
//        {
//            _mockDal.Setup(x => x.GetAll(
//                    It.IsAny<PaginationRequestDto>(),
//                    It.IsAny<string>(),
//                    It.IsAny<IDbTransaction>()))
//                .ReturnsAsync(new APIGetResponseModel<List<BranchModel>>
//                {
//                    IsSuccess = true,
//                    Result = new List<BranchModel>()
//                });

//            var result = await _bal.GetAll(
//                new PaginationRequestDto(),
//                new List<string> { "Super Admin" },
//                "test@test.com");

//            Assert.True(result.IsSuccess);
//        }

//        // =========================
//        // GET BY ID
//        // =========================
//        [Fact]
//        public async Task GetById_Should_Return_AccessDenied_When_Role_Is_Invalid()
//        {
//            var result = await _bal.GetById(
//                1,
//                new List<string> { "User" },
//                "test@test.com");

//            Assert.False(result.IsSuccess);
//            Assert.Contains("Access denied.", result.ErrorMsgs);
//        }

//        [Fact]
//        public async Task GetById_Should_Return_Data_When_Role_Is_Valid()
//        {
//            _mockDal.Setup(x => x.GetById(
//                    It.IsAny<long>(),
//                    It.IsAny<string>(),
//                    It.IsAny<IDbTransaction>()))
//                .ReturnsAsync(new APIGetResponseModel<BranchModel>
//                {
//                    IsSuccess = true,
//                    Result = new BranchModel()
//                });

//            var result = await _bal.GetById(
//                1,
//                new List<string> { "Super Admin" },
//                "test@test.com");

//            Assert.True(result.IsSuccess);
//        }

//        // =========================
//        // CREATE
//        // =========================
//        [Fact]
//        public async Task Create_Should_Return_Error_When_Role_Is_Invalid()
//        {
//            var request = new BranchRequestDto();

//            var result = await _bal.Create(
//                request,
//                new List<string> { "User" },
//                "test@test.com");

//            Assert.False(result.IsSuccess);
//            Assert.Contains("Only Super Admin and Org Admin can create branch.", result.ErrorMsgs);
//        }

//        [Fact]
//        public async Task Create_Should_Return_Error_When_Request_Is_Null()
//        {
//            BranchRequestDto request = null;

//            var result = await _bal.Create(
//                request,
//                new List<string> { "Super Admin" },
//                "test@test.com");

//            Assert.False(result.IsSuccess);
//            Assert.Contains("Invalid payload.", result.ErrorMsgs);
//        }

//        [Fact]
//        public async Task Create_Should_Return_Validation_Error_When_Required_Fields_Are_Missing()
//        {
//            var request = new BranchRequestDto();

//            var result = await _bal.Create(
//                request,
//                new List<string> { "Super Admin" },
//                "test@test.com");

//            Assert.False(result.IsSuccess);
//            Assert.Contains("Branch Name is required", result.ErrorMsgs);
//            Assert.Contains("Organization is required", result.ErrorMsgs);
//        }

//        [Fact]
//        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
//        {
//            var request = new BranchRequestDto
//            {
//                BranchName = "Main Branch",
//                OrganizationId = 1
//            };

//            _mockDal.Setup(x => x.Insert(
//                    It.IsAny<BranchRequestDto>(),
//                    It.IsAny<string>(),
//                    It.IsAny<IDbTransaction>()))
//                .ReturnsAsync(new APIGetResponseModel<int>
//                {
//                    IsSuccess = true,
//                    Result = 1
//                });

//            var result = await _bal.Create(
//                request,
//                new List<string> { "Super Admin" },
//                "test@test.com");

//            Assert.True(result.IsSuccess);
//            Assert.Equal(1, result.Result);
//        }

//        // =========================
//        // UPDATE
//        // =========================
//        [Fact]
//        public async Task Update_Should_Return_Error_When_Role_Is_Invalid()
//        {
//            var request = new BranchRequestDto();

//            var result = await _bal.Update(
//                request,
//                new List<string> { "User" },
//                "test@test.com");

//            Assert.False(result.IsSuccess);
//            Assert.Contains("Only Super Admin and Org Admin can update branch.", result.ErrorMsgs);
//        }

//        [Fact]
//        public async Task Update_Should_Return_Error_When_Request_Is_Invalid()
//        {
//            BranchRequestDto request = null;

//            var result = await _bal.Update(
//                request,
//                new List<string> { "Super Admin" },
//                "test@test.com");

//            Assert.False(result.IsSuccess);
//            Assert.Contains("Invalid branch data.", result.ErrorMsgs);
//        }

//        [Fact]
//        public async Task Update_Should_Return_Error_When_BranchName_Is_Missing()
//        {
//            var request = new BranchRequestDto
//            {
//                BranchId = 1,
//                BranchName = ""
//            };

//            var result = await _bal.Update(
//                request,
//                new List<string> { "Super Admin" },
//                "test@test.com");

//            Assert.False(result.IsSuccess);
//            Assert.Contains("Branch Name is required", result.ErrorMsgs);
//        }

//        [Fact]
//        public async Task Update_Should_Return_Success_When_Request_Is_Valid()
//        {
//            var request = new BranchRequestDto
//            {
//                BranchId = 1,
//                BranchName = "Updated Branch"
//            };

//            _mockDal.Setup(x => x.Update(
//                    It.IsAny<BranchRequestDto>(),
//                    It.IsAny<string>(),
//                    It.IsAny<IDbTransaction>()))
//                .ReturnsAsync(new APIGetResponseModel<int>
//                {
//                    IsSuccess = true,
//                    Result = 1
//                });

//            var result = await _bal.Update(
//                request,
//                new List<string> { "Super Admin" },
//                "test@test.com");

//            Assert.True(result.IsSuccess);
//        }

//        // =========================
//        // CHANGE STATUS
//        // =========================
//        [Fact]
//        public async Task ChangeStatus_Should_Return_Error_When_Role_Is_Invalid()
//        {
//            var result = await _bal.ChangeStatus(
//                1,
//                new List<string> { "Org Admin" },
//                "test@test.com");

//            Assert.False(result.IsSuccess);
//            Assert.Contains("Only Super Admin can delete branch.", result.ErrorMsgs);
//        }

//        [Fact]
//        public async Task ChangeStatus_Should_Return_Error_When_Id_Is_Invalid()
//        {
//            var result = await _bal.ChangeStatus(
//                0,
//                new List<string> { "Super Admin" },
//                "test@test.com");

//            Assert.False(result.IsSuccess);
//            Assert.Contains("Invalid branch ID.", result.ErrorMsgs);
//        }

//        [Fact]
//        public async Task ChangeStatus_Should_Return_Success_When_Request_Is_Valid()
//        {
//            _mockDal.Setup(x => x.ChangeStatus(
//                    It.IsAny<long>(),
//                    It.IsAny<string>(),
//                    It.IsAny<IDbTransaction>()))
//                .ReturnsAsync(new APIGetResponseModel<int>
//                {
//                    IsSuccess = true,
//                    Result = 1
//                });

//            var result = await _bal.ChangeStatus(
//                1,
//                new List<string> { "Super Admin" },
//                "test@test.com");

//            Assert.True(result.IsSuccess);
//        }

//        // =========================
//        // DROPDOWN
//        // =========================
//        [Fact]
//        public async Task GetDropdown_Should_Return_Data()
//        {
//            _mockDal.Setup(x => x.GetDropdown(
//                    It.IsAny<string>(),
//                    It.IsAny<IDbTransaction>()))
//                .ReturnsAsync(new APIGetResponseModel<List<DropdownModel>>
//                {
//                    IsSuccess = true,
//                    Result = new List<DropdownModel>()
//                });

//            var result = await _bal.GetDropdown("test@test.com");

//            Assert.True(result.IsSuccess);
//        }
//    }
//}