using BAL.ContractIF;
using BeeQAPI.Controllers;
using BeeQAPI.Realtime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Models;
using Moq;
using System.Security.Claims;
using Xunit;

namespace BeeQAPI.Tests.Controllers
{
    public class QueueControllerTests
    {
        private readonly Mock<IBAL_Queue> _balMock;
        private readonly Mock<IMonitorService> _monitorServiceMock;
        private readonly Mock<IHubContext<QueueHub>> _hubContextMock;
        private readonly Mock<IHubClients> _hubClientsMock;
        private readonly Mock<IClientProxy> _clientProxyMock;
        private readonly QueueController _controller;
    public QueueControllerTests()
        {
            _balMock = new Mock<IBAL_Queue>();
            _monitorServiceMock = new Mock<IMonitorService>();
            _hubContextMock = new Mock<IHubContext<QueueHub>>();
            _hubClientsMock = new Mock<IHubClients>();
            _clientProxyMock = new Mock<IClientProxy>();

            _hubContextMock.Setup(x => x.Clients).Returns(_hubClientsMock.Object);
            _hubClientsMock.Setup(x => x.Group(It.IsAny<string>())).Returns(_clientProxyMock.Object);

            _controller = new QueueController(_balMock.Object, _monitorServiceMock.Object, _hubContextMock.Object);

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "admin@test.com"),
            new Claim(ClaimTypes.Role, "Super Admin")
        };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = principal
                }
            };
        }

        [Fact]
        public async Task GetAll_ReturnsQueueList()
        {
            // Arrange
            var request = new PaginationRequestDto
            {
                PageNo = 1,
                PageSize = 10,
                SearchKey = ""
            };

            var expected = new APIGetResponseModel<List<QueueModel>>
            {
                IsSuccess = true,
                Result = new List<QueueModel>
        {
            new QueueModel { TokenId = 1 }
        }
            };

            _balMock
                .Setup(x => x.GetAll(
                    It.IsAny<PaginationRequestDto>(),
                    It.IsAny<List<string>>(),
                    It.IsAny<string>(),
                    null))
                .ReturnsAsync(expected);

            // Act
            var result = await _controller.GetAll(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Result);
        }
        [Fact]
        public async Task GetById_ReturnsQueueById()
        {
            // Arrange
            long id = 1;

            var expected = new APIGetResponseModel<QueueModel>
            {
                IsSuccess = true,
                Result = new QueueModel { TokenId = id }
            };

            _balMock
                .Setup(x => x.GetById(id, It.IsAny<List<string>>(), It.IsAny<string>(), null))
                .ReturnsAsync(expected);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(id, result.Result.TokenId);
        }

        [Fact]
        public async Task Create_ReturnsSuccess()
        {
            // Arrange
            var request = new QueueRequestDto
            {
                BranchId = 1,
                BranchServiceId = 2
            };

            var expected = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _balMock
                .Setup(x => x.Create(request, It.IsAny<List<string>>(), It.IsAny<string>(), null))
                .ReturnsAsync(expected);

            // Act
            var result = await _controller.Create(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }

        [Fact]
        public async Task Update_ReturnsSuccess()
        {
            // Arrange
            var request = new QueueRequestDto
            {
                TokenId = 1,
                BranchId = 1,
                BranchServiceId = 2
            };

            var expected = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _balMock
                .Setup(x => x.Update(request, It.IsAny<List<string>>(), It.IsAny<string>(), null))
                .ReturnsAsync(expected);

            // Act
            var result = await _controller.Update(request);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }

        [Fact]
        public async Task ChangeStatus_WhenSuccess_ShouldSendSignalRUpdate()
        {
            // Arrange
            var request = new QueueRequestDto
            {
                TokenId = 1,
                BranchId = 10
            };

            var statusResponse = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            var displayResponse = new APIGetResponseModel<List<QueueDisplayModel>>
            {
                IsSuccess = true,
                Result = new List<QueueDisplayModel>
            {
                new QueueDisplayModel()
            }
            };

            _balMock
                .Setup(x => x.ChangeStatus(request, It.IsAny<List<string>>(), It.IsAny<string>(), null))
                .ReturnsAsync(statusResponse);

            _balMock
                .Setup(x => x.GetQueueDisplay(request.BranchId.ToString()))
                .ReturnsAsync(displayResponse);

            // Act
            var result = await _controller.ChangeStatus(request);

            // Assert
            Assert.True(result.IsSuccess);

            _hubClientsMock.Verify(x => x.Group(request.BranchId.ToString()), Times.Once);

            _clientProxyMock.Verify(x =>
                x.SendCoreAsync(
                    "QueueUpdated",
                    It.Is<object[]>(o => o.Length == 1),
                    default),
                Times.Once);
        }

        [Fact]
        public async Task GetQueueDisplay_ReturnsDisplayData()
        {
            // Arrange
            long branchId = 5;

            var expected = new APIGetResponseModel<List<QueueDisplayModel>>
            {
                IsSuccess = true,
                Result = new List<QueueDisplayModel>
            {
                new QueueDisplayModel()
            }
            };

            _balMock
                .Setup(x => x.GetQueueDisplay(branchId.ToString()))
                .ReturnsAsync(expected);

            // Act
            var result = await _controller.GetQueueDisplay(branchId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Result);
        }

        [Fact]
        public async Task GetDropdown_ReturnsDropdownList()
        {
            // Arrange
            var expected = new APIGetResponseModel<List<DropdownModel>>
            {
                IsSuccess = true,
                Result = new List<DropdownModel>
            {
                new DropdownModel { Id = 1, Name = "Test" }
            }
            };

            _balMock
                .Setup(x => x.GetDropdown(It.IsAny<string>(), null))
                .ReturnsAsync(expected);

            // Act
            var result = await _controller.GetDropdown();

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Single(result.Result);
        }
    }


}
