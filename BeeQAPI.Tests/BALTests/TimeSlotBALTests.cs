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
    public class TimeSlotBALTests
    {
        private readonly Mock<IDAL_TimeSlot> _mockDal;
        private readonly BAL_TimeSlot _bal;

        public TimeSlotBALTests()
        {
            _mockDal = new Mock<IDAL_TimeSlot>();
            _bal = new BAL_TimeSlot(_mockDal.Object);
        }

        // ========================
        // GET ALL
        // ========================
        [Fact]
        public async Task GetAll_Should_Return_Data_When_DAL_Returns_Data()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal.Setup(x => x.GetAll(
                    It.IsAny<PaginationRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<List<TimeSlotModel>>
                {
                    IsSuccess = true,
                    Result = new List<TimeSlotModel>
                    {
                        new TimeSlotModel()
                    }
                });

            // Act
            var result = await _bal.GetAll(request, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.Single(result.Result);
        }

        [Fact]
        public async Task GetAll_Should_Throw_Exception_When_DAL_Throws_Exception()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal.Setup(x => x.GetAll(
                    It.IsAny<PaginationRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ThrowsAsync(new Exception("DB Error"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                _bal.GetAll(request, roles, email));

            Assert.Equal("BAL: Error in GetAll", ex.Message);
        }

        // ========================
        // GET BY ID
        // ========================
        [Fact]
        public async Task GetById_Should_Return_Data_When_DAL_Returns_Data()
        {
            // Arrange
            long id = 1;
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal.Setup(x => x.GetById(
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<TimeSlotModel>
                {
                    IsSuccess = true,
                    Result = new TimeSlotModel()
                });

            // Act
            var result = await _bal.GetById(id, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task GetById_Should_Throw_Exception_When_DAL_Throws_Exception()
        {
            // Arrange
            long id = 1;
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal.Setup(x => x.GetById(
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ThrowsAsync(new Exception("DB Error"));

            // Act & Assert
            var ex = await Assert.ThrowsAsync<Exception>(() =>
                _bal.GetById(id, roles, email));

            Assert.Equal("BAL: Error in GetById", ex.Message);
        }

        // ========================
        // CREATE
        // ========================
        [Fact]
        public async Task Create_Should_Return_Error_When_Request_Is_Null()
        {
            // Arrange
            TimeSlotRequestDto request = null;
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
            var request = new TimeSlotRequestDto
            {
                BranchId = 0,
                ServiceId = 0
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Create(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Branch required", result.ErrorMsgs);
            Assert.Contains("Service required", result.ErrorMsgs);

            // NOTE:
            // Do NOT check "Time required" here unless StartTime/EndTime are nullable (TimeSpan?)
            // because in your current BAL, null-check on TimeSpan won't work if DTO has non-nullable TimeSpan.
        }

        [Fact]
        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new TimeSlotRequestDto
            {
                BranchId = 1,
                ServiceId = 1,
                StartTime = TimeSpan.FromHours(10),
                EndTime = TimeSpan.FromHours(11)
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal.Setup(x => x.Insert(
                    It.IsAny<TimeSlotRequestDto>(),
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

        [Fact]
        public async Task Create_Should_Return_Error_When_DAL_Throws_Exception()
        {
            // Arrange
            var request = new TimeSlotRequestDto
            {
                BranchId = 1,
                ServiceId = 1,
                StartTime = TimeSpan.FromHours(10),
                EndTime = TimeSpan.FromHours(11)
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal.Setup(x => x.Insert(
                    It.IsAny<TimeSlotRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ThrowsAsync(new Exception("Insert failed"));

            // Act
            var result = await _bal.Create(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Insert failed", result.ErrorMsgs);
        }

        // ========================
        // UPDATE
        // ========================
        [Fact]
        public async Task Update_Should_Return_Error_When_Request_Is_Null()
        {
            // Arrange
            TimeSlotRequestDto request = null;
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Update(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid timeslot data.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_Error_When_SlotId_Is_Invalid()
        {
            // Arrange
            var request = new TimeSlotRequestDto
            {
                SlotId = 0
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.Update(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid timeslot data.", result.ErrorMsgs);
        }

        [Fact]
        public async Task Update_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new TimeSlotRequestDto
            {
                SlotId = 1,
                BranchId = 1,
                ServiceId = 1,
                StartTime = TimeSpan.FromHours(9),
                EndTime = TimeSpan.FromHours(10)
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal.Setup(x => x.Update(
                    It.IsAny<TimeSlotRequestDto>(),
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

        [Fact]
        public async Task Update_Should_Return_Error_When_DAL_Throws_Exception()
        {
            // Arrange
            var request = new TimeSlotRequestDto
            {
                SlotId = 1
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal.Setup(x => x.Update(
                    It.IsAny<TimeSlotRequestDto>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ThrowsAsync(new Exception("Update failed"));

            // Act
            var result = await _bal.Update(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Update failed", result.ErrorMsgs);
        }

        // ========================
        // CHANGE STATUS
        // ========================
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
            Assert.Contains("Invalid Slot ID.", result.ErrorMsgs);
        }

        [Fact]
        public async Task ChangeStatus_Should_Return_Success_When_Id_Is_Valid()
        {
            // Arrange
            long id = 1;
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal.Setup(x => x.ChangeStatus(
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ReturnsAsync(new APIGetResponseModel<int>
                {
                    IsSuccess = true,
                    Result = 1
                });

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

            _mockDal.Setup(x => x.ChangeStatus(
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ThrowsAsync(new Exception("Status update failed"));

            // Act
            var result = await _bal.ChangeStatus(id, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Status update failed", result.ErrorMsgs);
        }

        // ========================
        // DROPDOWN
        // ========================
        [Fact]
        public async Task GetDropdown_Should_Return_Data_When_DAL_Returns_Data()
        {
            // Arrange
            long serviceId = 1;
            var email = "test@test.com";

            _mockDal.Setup(x => x.GetDropdown(
                    It.IsAny<long>(),
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
            var result = await _bal.GetDropdown(serviceId, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.Single(result.Result);
        }

        [Fact]
        public async Task GetDropdown_Should_Return_Error_When_DAL_Throws_Exception()
        {
            // Arrange
            long serviceId = 1;
            var email = "test@test.com";

            _mockDal.Setup(x => x.GetDropdown(
                    It.IsAny<long>(),
                    It.IsAny<string>(),
                    It.IsAny<IDbTransaction>()))
                .ThrowsAsync(new Exception("Dropdown failed"));

            // Act
            var result = await _bal.GetDropdown(serviceId, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Dropdown failed", result.ErrorMsgs);
        }
    }
}