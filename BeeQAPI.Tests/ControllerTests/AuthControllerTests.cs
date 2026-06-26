using BAL.ContractIF;
using BeeQAPI.Controllers;
using Microsoft.AspNetCore.Http;
using Moq;
using Models;
using System.Security.Claims;
using Xunit;

namespace BeeQAPI.Tests
{
    public class AuthControllerTests
    {
        private readonly Mock<IBAL_Auth> _authMock;
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _authMock = new Mock<IBAL_Auth>();
            _controller = new AuthController(_authMock.Object);
        }

        // =========================================================
        // Helper: Set User Claims
        // =========================================================
        private void SetUserClaims(string userId)
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(userId))
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
            }

            var identity = new ClaimsIdentity(claims, "TestAuth");
            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = principal
                }
            };
        }

        // =========================================================
        // GET SALT
        // =========================================================
        [Fact]
        public async Task GetSalt_Should_Return_Salt_String()
        {
            // Arrange
            var expectedSalt = "RANDOM_SALT_123";

            _authMock
                .Setup(x => x.RandomString())
                .ReturnsAsync(expectedSalt);

            // Act
            var result = await _controller.GetSalt();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedSalt, result);
        }

        // =========================================================
        // LOGIN
        // =========================================================
        [Fact]
        public async Task Login_Should_Return_Login_Response_Successfully()
        {
            // Arrange
            var dto = new LoginRequestDto
            {
                Email = "test@mail.com",
                Password = "hashedpassword"
            };

            // first set salt through GetSalt()
            var salt = "mysalt";
            _authMock.Setup(x => x.RandomString()).ReturnsAsync(salt);
            await _controller.GetSalt();

            var expectedResponse = new APIGetResponseModel<ModelLoginResponse>
            {
                IsSuccess = true,
                Result = new ModelLoginResponse
                {
                    AuthToken = "jwt-token"
                },
                TotalRecords = 1
            };

            _authMock
                .Setup(x => x.Login(dto, salt, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Login(dto);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.Equal("jwt-token", result.Result.AuthToken);
        }

        [Fact]
        public async Task Login_Should_Return_Error_When_Login_Fails()
        {
            // Arrange
            var dto = new LoginRequestDto
            {
                Email = "wrong@mail.com",
                Password = "wrongpass"
            };

            var salt = "mysalt";
            _authMock.Setup(x => x.RandomString()).ReturnsAsync(salt);
            await _controller.GetSalt();

            var expectedResponse = new APIGetResponseModel<ModelLoginResponse>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Invalid email or password" },
                Result = new ModelLoginResponse()
            };

            _authMock
                .Setup(x => x.Login(dto, salt, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Login(dto);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid email or password", result.ErrorMsgs);
        }

        // =========================================================
        // LOGIN PROFILE
        // =========================================================
        [Fact]
        public async Task LoginProfile_Should_Return_User_Profile()
        {
            // Arrange
            var userId = "test@mail.com";
            SetUserClaims(userId);

            var expectedResponse = new APIGetResponseModel<UserProfileDetails>
            {
                IsSuccess = true,
                Result = new UserProfileDetails()
            };

            _authMock
                .Setup(x => x.loginprofile(userId, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.loginprofile();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task LoginProfile_Should_Pass_Null_When_UserId_Claim_Not_Found()
        {
            // Arrange
            SetUserClaims(null);

            var expectedResponse = new APIGetResponseModel<UserProfileDetails>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "User not found" }
            };

            _authMock
                .Setup(x => x.loginprofile(null, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.loginprofile();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("User not found", result.ErrorMsgs);
        }
    }
}