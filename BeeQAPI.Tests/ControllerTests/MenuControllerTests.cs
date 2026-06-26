using BAL.ContractIF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Models;
using System.Security.Claims;
using Xunit;

namespace BeeQAPI.Tests
{
    public class MenuControllerTests
    {
        private readonly Mock<IBAL_Menu> _balMock;
        private readonly MenuController _controller;

        public MenuControllerTests()
        {
            _balMock = new Mock<IBAL_Menu>();
            _controller = new MenuController(_balMock.Object);
        }

        private void SetUserClaims(string? email)
        {
            var claims = new List<Claim>();

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
        // GET SIDEBAR - SUCCESS
        // ========================
        [Fact]
        public async Task GetSidebar_Should_Return_Ok_When_Email_Claim_Exists()
        {
            // Arrange
            var email = "test@mail.com";
            SetUserClaims(email);

            var menuList = new List<MenuModel>
            {
                new MenuModel(),
                new MenuModel()
            };

            _balMock
                .Setup(x => x.GetSidebar(email))
                .ReturnsAsync(menuList);

            // Act
            var result = await _controller.GetSidebar();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsAssignableFrom<List<MenuModel>>(okResult.Value);

            Assert.Equal(2, value.Count);

            _balMock.Verify(x => x.GetSidebar(email), Times.Once);
        }

        // ========================
        // GET SIDEBAR - UNAUTHORIZED
        // ========================
        [Fact]
        public async Task GetSidebar_Should_Return_Unauthorized_When_Email_Claim_Is_Missing()
        {
            // Arrange
            SetUserClaims(null);

            // Act
            var result = await _controller.GetSidebar();

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid token", unauthorizedResult.Value);

            _balMock.Verify(x => x.GetSidebar(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task GetSidebar_Should_Return_Unauthorized_When_Email_Claim_Is_Empty()
        {
            // Arrange
            SetUserClaims("");

            // Act
            var result = await _controller.GetSidebar();

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Equal("Invalid token", unauthorizedResult.Value);

            _balMock.Verify(x => x.GetSidebar(It.IsAny<string>()), Times.Never);
        }

        // ========================
        // GET SIDEBAR - EXCEPTION
        // ========================
        [Fact]
        public async Task GetSidebar_Should_Return_500_When_Exception_Occurs()
        {
            // Arrange
            var email = "test@mail.com";
            SetUserClaims(email);

            _balMock
                .Setup(x => x.GetSidebar(email))
                .ThrowsAsync(new Exception("Something went wrong"));

            // Act
            var result = await _controller.GetSidebar();

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(500, objectResult.StatusCode);
            Assert.Contains("Something went wrong", objectResult.Value?.ToString());

            _balMock.Verify(x => x.GetSidebar(email), Times.Once);
        }
    }
}