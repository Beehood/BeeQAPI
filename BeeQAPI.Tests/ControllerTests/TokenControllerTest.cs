using BAL.ContractIF;
using BeeQAPI.Controllers;
using BeeQAPI.Realtime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Moq;
using Models;
using System.Security.Claims;
using Xunit;

namespace BeeQAPI.Tests.Controllers
{
    public class TokenControllerTests
    {
        private readonly Mock<IBAL_Token> _balMock;
        private readonly Mock<IHubContext<QueueHub>> _hubContextMock;
        private readonly Mock<IHubClients> _hubClientsMock;
        private readonly Mock<IClientProxy> _clientProxyMock;
        private readonly TokenController _controller;

    public TokenControllerTests()
        {
            _balMock = new Mock<IBAL_Token>();
            _hubContextMock = new Mock<IHubContext<QueueHub>>();
            _hubClientsMock = new Mock<IHubClients>();
            _clientProxyMock = new Mock<IClientProxy>();

            _hubContextMock.Setup(x => x.Clients).Returns(_hubClientsMock.Object);
            _hubClientsMock.Setup(x => x.Group(It.IsAny<string>())).Returns(_clientProxyMock.Object);

            _controller = new TokenController(_balMock.Object, _hubContextMock.Object);
        }

        private void SetUserClaims(List<string> roles, string? email)
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrEmpty(email))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, email));
            }

            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var user = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };
        }

        // ========================
        // GET ALL
        // ========================
        [Fact]
        public async Task GetAll_Should_Return_Token_List()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "admin@test.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<List<TokenModel>>
            {
                IsSuccess = true,
                Result = new List<TokenModel>
            {
                new TokenModel()
            }
            };

            _balMock
                .Setup(x => x.GetAll(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAll(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.Single(result.Result);

            _balMock.Verify(x => x.GetAll(request, roles, email, null), Times.Once);
        }

        // ========================
        // GET BY ID
        // ========================
        [Fact]
        public async Task GetById_Should_Return_Token_By_Id()
        {
            // Arrange
            long id = 1;
            var roles = new List<string> { "Super Admin" };
            var email = "admin@test.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<TokenModel>
            {
                IsSuccess = true,
                Result = new TokenModel()
            };

            _balMock
                .Setup(x => x.GetById(id, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);

            _balMock.Verify(x => x.GetById(id, roles, email, null), Times.Once);
        }

        // ========================
        // GENERATE TOKEN
        // ========================
        [Fact]
        public async Task GenerateToken_Should_Return_Success_And_Send_SignalR_Message()
        {
            // Arrange
            var request = new TokenRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "admin@test.com";
            long branchId = 1;

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<string>
            {
                IsSuccess = true,
                Result = "TKN001"
            };

            _balMock
                .Setup(x => x.GenerateToken(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            _balMock
                .Setup(x => x.GetBranchIdByEmail(email))
                .ReturnsAsync(branchId);

            _clientProxyMock
                .Setup(x => x.SendCoreAsync("QueueUpdated", It.IsAny<object[]>(), default))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.GenerateToken(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal("TKN001", result.Result);

            _balMock.Verify(x => x.GenerateToken(request, roles, email, null), Times.Once);
            _balMock.Verify(x => x.GetBranchIdByEmail(email), Times.Once);
            _hubClientsMock.Verify(x => x.Group(branchId.ToString()), Times.Once);
            _clientProxyMock.Verify(x => x.SendCoreAsync("QueueUpdated", It.IsAny<object[]>(), default), Times.Once);
        }

        [Fact]
        public async Task GenerateToken_Should_Not_Send_SignalR_Message_When_Failed()
        {
            // Arrange
            var request = new TokenRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "admin@test.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<string>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Failed" }
            };

            _balMock
                .Setup(x => x.GenerateToken(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GenerateToken(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);

            _balMock.Verify(x => x.GenerateToken(request, roles, email, null), Times.Once);
            _balMock.Verify(x => x.GetBranchIdByEmail(It.IsAny<string>()), Times.Never);
            _hubClientsMock.Verify(x => x.Group(It.IsAny<string>()), Times.Never);
        }

        // ========================
        // CHANGE STATUS
        // ========================
        [Fact]
        public async Task ChangeStatus_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new TokenRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "admin@test.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _balMock
                .Setup(x => x.ChangeStatus(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.ChangeStatus(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);

            _balMock.Verify(x => x.ChangeStatus(request, roles, email, null), Times.Once);
        }

        // ========================
        // CALL NEXT TOKEN
        // ========================
        [Fact]
        public async Task CallNextToken_Should_Return_Token()
        {
            // Arrange
            var request = new TokenRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "admin@test.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<TokenModel>
            {
                IsSuccess = true,
                Result = new TokenModel()
            };

            _balMock
                .Setup(x => x.CallNextToken(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CallNextToken(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);

            _balMock.Verify(x => x.CallNextToken(request, roles, email, null), Times.Once);
        }

        // ========================
        // STATUS LIST
        // ========================
        [Fact]
        public async Task GetStatuses_Should_Return_Status_List()
        {
            // Arrange
            var email = "admin@test.com";

            SetUserClaims(new List<string> { "Super Admin" }, email);

            var expectedResponse = new APIGetResponseModel<List<TokenStatusModel>>
            {
                IsSuccess = true,
                Result = new List<TokenStatusModel>
            {
                new TokenStatusModel()
            }
            };

            _balMock
                .Setup(x => x.GetStatuses(email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetStatuses();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.Single(result.Result);

            _balMock.Verify(x => x.GetStatuses(email, null), Times.Once);
        }

        // ========================
        // DROPDOWN
        // ========================
        [Fact]
        public async Task GetDropdown_Should_Return_Dropdown_List()
        {
            // Arrange
            var email = "admin@test.com";

            SetUserClaims(new List<string> { "Super Admin" }, email);

            var expectedResponse = new APIGetResponseModel<List<DropdownModel>>
            {
                IsSuccess = true,
                Result = new List<DropdownModel>
            {
                new DropdownModel()
            }
            };

            _balMock
                .Setup(x => x.GetDropdown(email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetDropdown();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.Single(result.Result);

            _balMock.Verify(x => x.GetDropdown(email, null), Times.Once);
        }

        // ========================
        // NEXT TOKEN PREVIEW
        // ========================
        [Fact]
        public async Task NextTokenPreview_Should_Return_Token_Preview()
        {
            // Arrange
            var request = new TokenRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "admin@test.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<TokenModel>
            {
                IsSuccess = true,
                Result = new TokenModel()
            };

            _balMock
                .Setup(x => x.NextTokenPreview(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.NextTokenPreview(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);

            _balMock.Verify(x => x.NextTokenPreview(request, roles, email, null), Times.Once);
        }

        // ========================
        // EXTRA CLAIM TESTS
        // ========================
        [Fact]
        public async Task GetAll_Should_Pass_Empty_Roles_When_No_Role_Claims()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var email = "admin@test.com";

            SetUserClaims(new List<string>(), email);

            var expectedResponse = new APIGetResponseModel<List<TokenModel>>
            {
                IsSuccess = true,
                Result = new List<TokenModel>()
            };

            _balMock
                .Setup(x => x.GetAll(request, It.Is<List<string>>(r => r.Count == 0), email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAll(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            _balMock.Verify(x => x.GetAll(request, It.Is<List<string>>(r => r.Count == 0), email, null), Times.Once);
        }

        [Fact]
        public async Task GenerateToken_Should_Pass_Null_Email_When_NameIdentifier_Not_Present()
        {
            // Arrange
            var request = new TokenRequestDto();

            var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Role, "Super Admin")
        };

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var user = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

            var roles = new List<string> { "Super Admin" };

            var expectedResponse = new APIGetResponseModel<string>
            {
                IsSuccess = true,
                Result = "TKN001"
            };

            _balMock
                .Setup(x => x.GenerateToken(request, roles, null, null))
                .ReturnsAsync(expectedResponse);

            _balMock
                .Setup(x => x.GetBranchIdByEmail(null))
                .ReturnsAsync(1);

            _clientProxyMock
                .Setup(x => x.SendCoreAsync("QueueUpdated", It.IsAny<object[]>(), default))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.GenerateToken(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            _balMock.Verify(x => x.GenerateToken(request, roles, null, null), Times.Once);
        }
    }


}
