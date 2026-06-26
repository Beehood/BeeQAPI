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
    public class DisplayBoardServiceControllerTests
    {
        private readonly Mock<IBAL_DisplayBoardService> _balMock;
        private readonly DisplayBoardServiceController _controller;

        public DisplayBoardServiceControllerTests()
        {
            _balMock = new Mock<IBAL_DisplayBoardService>();
            _controller = new DisplayBoardServiceController(_balMock.Object);
        }

        private void SetUserClaims(List<string> roles, string? email)
        {
            var claims = new List<Claim>();

            if (roles != null)
            {
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
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
        // GET ALL
        // ========================
        [Fact]
        public async Task GetAll_Should_Return_DisplayBoardService_List()
        {
            // Arrange
            long displayId = 1;
            var roles = new List<string> { "Super Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<List<DisplayBoardServiceModel>>
            {
                IsSuccess = true,
                Result = new List<DisplayBoardServiceModel>
                {
                    new DisplayBoardServiceModel()
                }
            };

            _balMock
                .Setup(x => x.GetAll(displayId, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAll(displayId);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);

            _balMock.Verify(x => x.GetAll(displayId, roles, email, null), Times.Once);
        }

        [Fact]
        public async Task GetAll_Should_Pass_Empty_Roles_When_No_Role_Claims()
        {
            // Arrange
            long displayId = 1;
            var roles = new List<string>();
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<List<DisplayBoardServiceModel>>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Access denied." }
            };

            _balMock
                .Setup(x => x.GetAll(displayId, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAll(displayId);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);

            _balMock.Verify(x => x.GetAll(displayId, roles, email, null), Times.Once);
        }

        // ========================
        // CREATE
        // ========================
        [Fact]
        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new DisplayBoardServiceRequestDto
            {
                DisplayId = 1,
                BranchServiceId = 10
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

        [Fact]
        public async Task Create_Should_Pass_Null_Email_When_NameIdentifier_Claim_Missing()
        {
            // Arrange
            var request = new DisplayBoardServiceRequestDto
            {
                DisplayId = 1,
                BranchServiceId = 10
            };

            var roles = new List<string> { "Super Admin" };
            SetUserClaims(roles, null);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Invalid user." }
            };

            _balMock
                .Setup(x => x.Create(request, roles, null, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Create(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid user.", result.ErrorMsgs);

            _balMock.Verify(x => x.Create(request, roles, null, null), Times.Once);
        }

        // ========================
        // DELETE
        // ========================
        [Fact]
        public async Task Delete_Should_Return_Success_When_Id_Is_Valid()
        {
            // Arrange
            long id = 5;
            var roles = new List<string> { "Super Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _balMock
                .Setup(x => x.Delete(id, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);

            _balMock.Verify(x => x.Delete(id, roles, email, null), Times.Once);
        }

        [Fact]
        public async Task Delete_Should_Pass_Claims_To_BAL_Correctly()
        {
            // Arrange
            long id = 7;
            var roles = new List<string> { "Branch Admin" };
            var email = "branchadmin@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Only Super Admin can delete mapping." }
            };

            _balMock
                .Setup(x => x.Delete(id, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Only Super Admin can delete mapping.", result.ErrorMsgs);

            _balMock.Verify(x => x.Delete(id, roles, email, null), Times.Once);
        }
    }
}