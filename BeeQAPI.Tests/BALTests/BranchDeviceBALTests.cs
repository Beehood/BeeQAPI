using Xunit;
using Moq;
using BAL.Services;
using DAL.ContractIF;
using Models;
using System.Data;

namespace BeeQAPI.Tests.BALTests
{
    public class BranchDeviceBALTests
    {
        private readonly Mock<IDAL_BranchDevice> _mockDal;
        private readonly BAL_BranchDevice _bal;

        public BranchDeviceBALTests()
        {
            _mockDal = new Mock<IDAL_BranchDevice>();
            _bal = new BAL_BranchDevice(_mockDal.Object);
        }

        // =========================
        // GET ALL
        // =========================
        [Fact]
        public async Task GetAll_Should_Return_AccessDenied_When_Role_Is_Invalid()
        {
            var request = new PaginationRequestDto();
            var roles = new List<string> { "User" };

            var result = await _bal.GetAll(request, roles, "test@test.com");

            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetAll_Should_Return_Data_When_Role_Is_Valid()
        {
            _mockDal.Setup(x => x.GetAll(
                    It.IsAny<PaginationRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<List<DeviceModel>>
                {
                    IsSuccess = true,
                    Result = new List<DeviceModel>()
                });

            var result = await _bal.GetAll(
                new PaginationRequestDto(),
                new List<string> { "Super Admin" },
                "test@test.com");

            Assert.True(result.IsSuccess);
        }

        // =========================
        // GET BY ID
        // =========================
        [Fact]
        public async Task GetById_Should_Return_AccessDenied_When_Role_Is_Invalid()
        {
            var result = await _bal.GetById(
                1,
                new List<string> { "User" },
                "test@test.com");

            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetById_Should_Return_Data_When_Role_Is_Valid()
        {
            _mockDal.Setup(x => x.GetById(
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<DeviceModel>
                {
                    IsSuccess = true,
                    Result = new DeviceModel()
                });

            var result = await _bal.GetById(
                1,
                new List<string> { "Super Admin" },
                "test@test.com");

            Assert.True(result.IsSuccess);
        }

        // =========================
        // CREATE
        // =========================
        [Fact]
        public async Task Create_Should_Return_Error_When_Role_Is_Invalid()
        {
            var request = new DeviceRequestDto();

            var result = await _bal.Create(
                request,
                new List<string> { "Branch User" },
                "test@test.com");

            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_Error_When_Request_Is_Null()
        {
            DeviceRequestDto request = null;

            var result = await _bal.Create(
                request,
                new List<string> { "Super Admin" },
                "test@test.com");

            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid payload.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_Validation_Error_When_Required_Fields_Are_Missing()
        {
            var request = new DeviceRequestDto();

            var result = await _bal.Create(
                request,
                new List<string> { "Super Admin" },
                "test@test.com");

            Assert.False(result.IsSuccess);
            Assert.Contains("Device Name is required", result.ErrorMsgs);
            Assert.Contains("Device Type is required", result.ErrorMsgs);
        }

        [Fact]
        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
        {
            var request = new DeviceRequestDto
            {
                DeviceName = "Scanner 1",
                DeviceType = "Scanner"
            };

            _mockDal.Setup(x => x.Insert(
                    It.IsAny<DeviceRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<int>
                {
                    IsSuccess = true,
                    Result = 1
                });

            var result = await _bal.Create(
                request,
                new List<string> { "Super Admin" },
                "test@test.com");

            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }

        // =========================
        // UPDATE
        // =========================
        [Fact]
        public async Task Update_Should_Return_Error_When_Role_Is_Invalid()
        {
            var request = new DeviceRequestDto();

            var result = await _bal.Update(
                request,
                new List<string> { "Branch User" },
                "test@test.com");

            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_Error_When_DeviceId_Is_Invalid()
        {
            var request = new DeviceRequestDto
            {
                DeviceId = 0
            };

            var result = await _bal.Update(
                request,
                new List<string> { "Super Admin" },
                "test@test.com");

            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid Device Id.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_Success_When_Request_Is_Valid()
        {
            var request = new DeviceRequestDto
            {
                DeviceId = 1,
                DeviceName = "Printer 1",
                DeviceType = "Printer"
            };

            _mockDal.Setup(x => x.Update(
                    It.IsAny<DeviceRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<int>
                {
                    IsSuccess = true,
                    Result = 1
                });

            var result = await _bal.Update(
                request,
                new List<string> { "Super Admin" },
                "test@test.com");

            Assert.True(result.IsSuccess);
        }

        // =========================
        // CHANGE STATUS
        // =========================
        [Fact]
        public async Task ChangeStatus_Should_Return_Error_When_Role_Is_Invalid()
        {
            var result = await _bal.ChangeStatus(
                1,
                new List<string> { "Branch User" },
                "test@test.com");

            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task ChangeStatus_Should_Return_Error_When_Id_Is_Invalid()
        {
            var result = await _bal.ChangeStatus(
                0,
                new List<string> { "Super Admin" },
                "test@test.com");

            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid Device Id.", result.ErrorMsgs);
        }

        [Fact]
        public async Task ChangeStatus_Should_Return_Success_When_Request_Is_Valid()
        {
            _mockDal.Setup(x => x.ChangeStatus(
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<int>
                {
                    IsSuccess = true,
                    Result = 1
                });

            var result = await _bal.ChangeStatus(
                1,
                new List<string> { "Super Admin" },
                "test@test.com");

            Assert.True(result.IsSuccess);
        }

        // =========================
        // DROPDOWN
        // =========================
        [Fact]
        public async Task GetDropdown_Should_Return_Data()
        {
            _mockDal.Setup(x => x.GetDropdown(
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<List<DropdownModel>>
                {
                    IsSuccess = true,
                    Result = new List<DropdownModel>()
                });

            var result = await _bal.GetDropdown("test@test.com");

            Assert.True(result.IsSuccess);
        }
    }
}