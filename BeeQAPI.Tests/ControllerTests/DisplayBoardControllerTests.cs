using BAL.ContractIF;
using BeeQAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Models;
using System.Security.Claims;
using Xunit;

namespace BeeQAPI.Tests
{
    public class DisplayBoardControllerTests
    {
        private readonly Mock<IBAL_DisplayBoard> _balMock;
        private readonly DisplayBoardController _controller;

        public DisplayBoardControllerTests()
        {
            _balMock = new Mock<IBAL_DisplayBoard>();
            _controller = new DisplayBoardController(_balMock.Object);
        }

        private void SetUserClaims(List<string> roles, string? email)
        {
            var claims = new List<Claim>();

            if (roles != null)
            {
                claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, email));
            }

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

        // ========================
        // DISPLAY BOARD VIEW
        // ========================
        [Fact]
        public async Task DisplayBoardView_Should_Return_Display_Data()
        {
            // Arrange
            var email = "test@mail.com";
            SetUserClaims(new List<string>(), email);

            var expectedResponse = new APIGetResponseModel<List<QueueDisplayModel>>
            {
                IsSuccess = true,
                Result = new List<QueueDisplayModel>
                {
                    new QueueDisplayModel()
                }
            };

            _balMock
                .Setup(x => x.GetDisplayData(email))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.DisplayBoardView();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);

            _balMock.Verify(x => x.GetDisplayData(email), Times.Once);
        }

        [Fact]
        public async Task DisplayBoardView_Should_Pass_Null_When_Email_Claim_Missing()
        {
            // Arrange
            SetUserClaims(new List<string>(), null);

            var expectedResponse = new APIGetResponseModel<List<QueueDisplayModel>>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "User not found." }
            };

            _balMock
                .Setup(x => x.GetDisplayData(null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.DisplayBoardView();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("User not found.", result.ErrorMsgs);

            _balMock.Verify(x => x.GetDisplayData(null), Times.Once);
        }

        // ========================
        // GET ALL
        // ========================
        [Fact]
        public async Task GetAll_Should_Return_DisplayBoard_List()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<List<DisplayBoardModel>>
            {
                IsSuccess = true,
                Result = new List<DisplayBoardModel>
                {
                    new DisplayBoardModel()
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

            _balMock.Verify(x => x.GetAll(request, roles, email, null), Times.Once);
        }

        // ========================
        // GET BY ID
        // ========================
        [Fact]
        public async Task GetById_Should_Return_DisplayBoard_By_Id()
        {
            // Arrange
            long id = 1;
            var roles = new List<string> { "Super Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<DisplayBoardModel>
            {
                IsSuccess = true,
                Result = new DisplayBoardModel()
            };

            _balMock
                .Setup(x => x.GetById(id, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);

            _balMock.Verify(x => x.GetById(id, roles, email, null), Times.Once);
        }

        // ========================
        // CREATE
        // ========================
        [Fact]
        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new DisplayBoardRequestDto
            {
                DisplayName = "Main Display",
                ScreenCode = "SCR001",
                BranchId = 1
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _balMock
                .Setup(x => x.Create(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Create(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);

            _balMock.Verify(x => x.Create(request, roles, email, null), Times.Once);
        }

        // ========================
        // UPDATE
        // ========================
        [Fact]
        public async Task Update_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new DisplayBoardRequestDto
            {
                DisplayId = 1,
                DisplayName = "Updated Display",
                ScreenCode = "SCR002",
                BranchId = 1
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _balMock
                .Setup(x => x.Update(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Update(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);

            _balMock.Verify(x => x.Update(request, roles, email, null), Times.Once);
        }

        // ========================
        // STATUS
        // ========================
        [Fact]
        public async Task ChangeStatus_Should_Return_Success_When_DisplayId_Is_Valid()
        {
            // Arrange
            var request = new DisplayBoardRequestDto
            {
                DisplayId = 1
            };

            var roles = new List<string> { "Super Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _balMock
                .Setup(x => x.ChangeStatus(request.DisplayId, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.ChangeStatus(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);

            _balMock.Verify(x => x.ChangeStatus(request.DisplayId, roles, email, null), Times.Once);
        }

        // ========================
        // DROPDOWN
        // ========================
        [Fact]
        public async Task GetDropdown_Should_Return_DisplayBoard_Dropdown()
        {
            // Arrange
            var email = "test@mail.com";
            SetUserClaims(new List<string>(), email);

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

            _balMock.Verify(x => x.GetDropdown(email, null), Times.Once);
        }
    }
}