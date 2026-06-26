using Xunit;
using Moq;
using BAL.Services;
using BAL.ContractIF;
using DAL.ContractIF;
using Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BeeQAPI.Tests.BALTests
{
    public class BAL_AuthTests
    {
        private readonly Mock<IDAL_Auth> _dalMock;
        private readonly Mock<IJwtService> _jwtMock;
        private readonly BAL_Auth _bal;

        public BAL_AuthTests()
        {
            _dalMock = new Mock<IDAL_Auth>();
            _jwtMock = new Mock<IJwtService>();
            _bal = new BAL_Auth(_dalMock.Object, _jwtMock.Object);
        }

        // =========================
        // LOGIN TESTS
        // =========================

        [Fact]
        public async Task Login_Should_Return_Fail_When_Dto_Is_Null()
        {
            // Arrange
            string salt = "abc123";

            // Act
            var result = await _bal.Login(null, salt);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid login data", result.ErrorMsgs);
        }

        [Fact]
        public async Task Login_Should_Return_Fail_When_Email_Is_Empty()
        {
            // Arrange
            var dto = new LoginRequestDto
            {
                Email = "",
                Password = "test123"
            };

            // Act
            var result = await _bal.Login(dto, "salt123");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid login data", result.ErrorMsgs);
        }

        [Fact]
        public async Task Login_Should_Return_Fail_When_Password_Is_Empty()
        {
            // Arrange
            var dto = new LoginRequestDto
            {
                Email = "test@mail.com",
                Password = ""
            };

            // Act
            var result = await _bal.Login(dto, "salt123");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid login data", result.ErrorMsgs);
        }

        [Fact]
        public async Task Login_Should_Return_Fail_When_User_Not_Found()
        {
            // Arrange
            var dto = new LoginRequestDto
            {
                Email = "test@mail.com",
                Password = "somepassword"
            };

            var dalResponse = new APIGetResponseModel<UserDetails>
            {
                IsSuccess = false,
                Result = null
            };

            _dalMock
                .Setup(x => x.ValidateUser(dto.Email, null))
                .ReturnsAsync(dalResponse);

            // Act
            var result = await _bal.Login(dto, "salt123");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid email or password", result.ErrorMsgs);
        }

        [Fact]
        public async Task Login_Should_Return_Fail_When_Password_Is_Invalid()
        {
            // Arrange
            string salt = "mysalt";
            string dbStoredPassword = "db-password";

            var dto = new LoginRequestDto
            {
                Email = "test@mail.com",
                Password = "wrong-password-hash"
            };

            var user = new UserDetails
            {
                UserName = "swap",
                Name = "Swapnalisa",
                Password = dbStoredPassword,
                OrganizationId = 1,
                BranchId = 2,
                CounterId = 3,
                Roles = new List<string> { "Super Admin" },
                Permissions = new List<string> { "VIEW_USER" }
            };

            var dalResponse = new APIGetResponseModel<UserDetails>
            {
                IsSuccess = true,
                Result = user
            };

            _dalMock
                .Setup(x => x.ValidateUser(dto.Email, null))
                .ReturnsAsync(dalResponse);

            // Act
            var result = await _bal.Login(dto, salt);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid password", result.ErrorMsgs);
        }

        [Fact]
        public async Task Login_Should_Return_Success_When_Credentials_Are_Valid()
        {
            // Arrange
            string salt = "mysalt";
            string dbStoredPassword = "db-password";

            // BAL logic:
            // saltedInput = salt + user.Password
            // inputHash = GetHashString(saltedInput)
            // if (inputHash != dto.Password) => fail
            string correctHash = BAL_Auth.GetHashString(salt + dbStoredPassword);

            var dto = new LoginRequestDto
            {
                Email = "test@mail.com",
                Password = correctHash
            };

            var user = new UserDetails
            {
                UserName = "swap",
                Name = "Swapnalisa",
                Password = dbStoredPassword,
                OrganizationId = 1,
                BranchId = 2,
                CounterId = 3,
                Roles = new List<string> { "Super Admin" },
                Permissions = new List<string> { "VIEW_USER", "CREATE_USER" }
            };

            var dalResponse = new APIGetResponseModel<UserDetails>
            {
                IsSuccess = true,
                Result = user
            };

            _dalMock
                .Setup(x => x.ValidateUser(dto.Email, null))
                .ReturnsAsync(dalResponse);

            _jwtMock
                .Setup(x => x.GenerateTokenAsync(It.IsAny<TokenUserInfo>()))
                .ReturnsAsync("dummy-jwt-token");

            // Act
            var result = await _bal.Login(dto, salt);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.Equal("dummy-jwt-token", result.Result.AuthToken);
            Assert.Equal(1, result.TotalRecords);

            _jwtMock.Verify(x => x.GenerateTokenAsync(It.IsAny<TokenUserInfo>()), Times.Once);
        }

        [Fact]
        public async Task Login_Should_Return_Fail_When_Exception_Occurs()
        {
            // Arrange
            var dto = new LoginRequestDto
            {
                Email = "test@mail.com",
                Password = "password"
            };

            _dalMock
                .Setup(x => x.ValidateUser(dto.Email, null))
                .ThrowsAsync(new Exception("DB error"));

            // Act
            var result = await _bal.Login(dto, "salt123");

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Something went wrong", result.ErrorMsgs);
        }

        // =========================
        // LOGIN PROFILE TEST
        // =========================

        [Fact]
        public async Task LoginProfile_Should_Return_Profile_Data()
        {
            // Arrange
            string userId = "1";

            var profileResponse = new APIGetResponseModel<UserProfileDetails>
            {
                IsSuccess = true,
                Result = new UserProfileDetails()
            };

            _dalMock
                .Setup(x => x.loginprofile(userId, null))
                .ReturnsAsync(profileResponse);

            // Act
            var result = await _bal.loginprofile(userId);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        // =========================
        // HASH TEST
        // =========================

        [Fact]
        public void GetHashString_Should_Return_Hash_Value()
        {
            // Arrange
            string input = "test123";

            // Act
            string hash = BAL_Auth.GetHashString(input);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(hash));
            Assert.NotEqual(input, hash);
        }

        [Fact]
        public void GetHashString_Should_Return_Same_Hash_For_Same_Input()
        {
            // Arrange
            string input = "same-password";

            // Act
            string hash1 = BAL_Auth.GetHashString(input);
            string hash2 = BAL_Auth.GetHashString(input);

            // Assert
            Assert.Equal(hash1, hash2);
        }

        // =========================
        // RANDOM STRING TEST
        // =========================

        [Fact]
        public async Task RandomString_Should_Return_32_Char_String()
        {
            // Act
            var result = await _bal.RandomString();

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(result));
            Assert.Equal(32, result.Length);
        }

        [Fact]
        public async Task RandomString_Should_Return_Different_Values()
        {
            // Act
            var str1 = await _bal.RandomString();
            var str2 = await _bal.RandomString();

            // Assert
            Assert.NotEqual(str1, str2);
        }
    }
}