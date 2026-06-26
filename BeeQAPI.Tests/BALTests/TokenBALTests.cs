using BAL.Services;
using DAL.ContractIF;
using Microsoft.Extensions.Logging;
using Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Xunit;

namespace BeeQAPI.Tests.BALTests
{
    public class TokenBALTests
    {
        private readonly Mock<IDAL_Token> _mockDal;
        private readonly Mock<ILogger<BAL_Token>> _mockLogger;
        private readonly BAL_Token _bal;

        public TokenBALTests()
        {
            _mockDal = new Mock<IDAL_Token>();
            _mockLogger = new Mock<ILogger<BAL_Token>>();
            _bal = new BAL_Token(_mockDal.Object, _mockLogger.Object);
        }

        // ========================
        // GET BRANCH ID BY EMAIL
        // ========================
        [Fact]
        public async Task GetBranchIdByEmail_Should_Return_BranchId()
        {
            // Arrange
            var email = "test@test.com";
            _mockDal.Setup(x => x.GetBranchIdByEmail(email)).ReturnsAsync(5);

            // Act
            var result = await _bal.GetBranchIdByEmail(email);

            // Assert
            Assert.Equal(5, result);
        }

        // ========================
        // GET ALL
        // ========================
        [Fact]
        public async Task GetAll_Should_Return_AccessDenied_When_Roles_Are_Empty()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var roles = new List<string>();
            var email = "test@test.com";

            // Act
            var result = await _bal.GetAll(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetAll_Should_Return_Data_When_Request_Is_Valid()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            var expected = new APIGetResponseModel<List<TokenModel>>
            {
                IsSuccess = true,
                Result = new List<TokenModel> { new TokenModel() }
            };

            _mockDal.Setup(x => x.GetAll(request, email, null)).ReturnsAsync(expected);

            // Act
            var result = await _bal.GetAll(request, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.Single(result.Result);
        }

        [Fact]
        public async Task GetAll_Should_Return_Error_When_DAL_Throws_Exception()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal.Setup(x => x.GetAll(request, email, null))
                    .ThrowsAsync(new Exception("DB Error"));

            // Act
            var result = await _bal.GetAll(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Error while fetching tokens", result.ErrorMsgs);
        }

        // ========================
        // GET BY ID
        // ========================
        [Fact]
        public async Task GetById_Should_Return_AccessDenied_When_Roles_Are_Empty()
        {
            // Arrange
            long id = 1;
            var roles = new List<string>();
            var email = "test@test.com";

            // Act
            var result = await _bal.GetById(id, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GetById_Should_Return_Data_When_Request_Is_Valid()
        {
            // Arrange
            long id = 1;
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            var expected = new APIGetResponseModel<TokenModel>
            {
                IsSuccess = true,
                Result = new TokenModel()
            };

            _mockDal.Setup(x => x.GetById(id, email, null)).ReturnsAsync(expected);

            // Act
            var result = await _bal.GetById(id, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task GetById_Should_Return_Error_When_DAL_Throws_Exception()
        {
            // Arrange
            long id = 1;
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal.Setup(x => x.GetById(id, email, null))
                    .ThrowsAsync(new Exception("DB Error"));

            // Act
            var result = await _bal.GetById(id, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Error while fetching token", result.ErrorMsgs);
        }

        // ========================
        // GENERATE TOKEN
        // ========================
        [Fact]
        public async Task GenerateToken_Should_Return_AccessDenied_When_Roles_Are_Empty()
        {
            // Arrange
            var request = new TokenRequestDto();
            var roles = new List<string>();
            var email = "test@test.com";

            // Act
            var result = await _bal.GenerateToken(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GenerateToken_Should_Return_Error_When_Request_Is_Null()
        {
            // Arrange
            TokenRequestDto request = null;
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.GenerateToken(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid payload.", result.ErrorMsgs);
        }

        [Fact]
        public async Task GenerateToken_Should_Return_ValidationErrors_When_Required_Fields_Are_Missing()
        {
            // Arrange
            var request = new TokenRequestDto
            {
                BranchServiceId = 0,
                CustomerName = ""
            };
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.GenerateToken(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Branch Service required", result.ErrorMsgs);
            Assert.Contains("Customer Name required", result.ErrorMsgs);
        }

        [Fact]
        public async Task GenerateToken_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new TokenRequestDto
            {
                BranchServiceId = 1,
                CustomerName = "Swapna"
            };
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            var expected = new APIGetResponseModel<string>
            {
                IsSuccess = true,
                Result = "T001"
            };

            _mockDal.Setup(x => x.GenerateToken(request, email, null)).ReturnsAsync(expected);

            // Act
            var result = await _bal.GenerateToken(request, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal("T001", result.Result);
        }

        [Fact]
        public async Task GenerateToken_Should_Return_Error_When_DAL_Throws_Exception()
        {
            // Arrange
            var request = new TokenRequestDto
            {
                BranchServiceId = 1,
                CustomerName = "Swapna"
            };
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal.Setup(x => x.GenerateToken(request, email, null))
                    .ThrowsAsync(new Exception("DB Error"));

            // Act
            var result = await _bal.GenerateToken(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Error while generating token", result.ErrorMsgs);
        }

        // ========================
        // CHANGE STATUS
        // ========================
        [Fact]
        public async Task ChangeStatus_Should_Return_AccessDenied_When_Roles_Are_Empty()
        {
            // Arrange
            var request = new TokenRequestDto();
            var roles = new List<string>();
            var email = "test@test.com";

            // Act
            var result = await _bal.ChangeStatus(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task ChangeStatus_Should_Return_Error_When_TokenId_Is_Invalid()
        {
            // Arrange
            var request = new TokenRequestDto { TokenId = 0 };
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.ChangeStatus(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid TokenId.", result.ErrorMsgs);
        }

        [Fact]
        public async Task ChangeStatus_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new TokenRequestDto { TokenId = 1 };
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            var expected = new APIGetResponseModel<int>
            {
                IsSuccess = true,
                Result = 1
            };

            _mockDal.Setup(x => x.ChangeStatus(request, email, null)).ReturnsAsync(expected);

            // Act
            var result = await _bal.ChangeStatus(request, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }

        [Fact]
        public async Task ChangeStatus_Should_Return_Error_When_DAL_Throws_Exception()
        {
            // Arrange
            var request = new TokenRequestDto { TokenId = 1 };
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal.Setup(x => x.ChangeStatus(request, email, null))
                    .ThrowsAsync(new Exception("DB Error"));

            // Act
            var result = await _bal.ChangeStatus(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Error while updating token status", result.ErrorMsgs);
        }

        // ========================
        // CALL NEXT TOKEN
        // ========================
        [Fact]
        public async Task CallNextToken_Should_Return_AccessDenied_When_Roles_Are_Empty()
        {
            // Arrange
            var request = new TokenRequestDto();
            var roles = new List<string>();
            var email = "test@test.com";

            // Act
            var result = await _bal.CallNextToken(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task CallNextToken_Should_Return_Error_When_Request_Is_Null()
        {
            // Arrange
            TokenRequestDto request = null;
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.CallNextToken(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid payload.", result.ErrorMsgs);
        }

        [Fact]
        public async Task CallNextToken_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new TokenRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            var expected = new APIGetResponseModel<TokenModel>
            {
                IsSuccess = true,
                Result = new TokenModel()
            };

            _mockDal.Setup(x => x.CallNextToken(request, email, null)).ReturnsAsync(expected);

            // Act
            var result = await _bal.CallNextToken(request, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task CallNextToken_Should_Return_Error_When_DAL_Throws_Exception()
        {
            // Arrange
            var request = new TokenRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal.Setup(x => x.CallNextToken(request, email, null))
                    .ThrowsAsync(new Exception("DB Error"));

            // Act
            var result = await _bal.CallNextToken(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Error while calling next token", result.ErrorMsgs);
        }

        // ========================
        // GET STATUSES
        // ========================
        [Fact]
        public async Task GetStatuses_Should_Return_Data()
        {
            // Arrange
            var email = "test@test.com";
            var expected = new APIGetResponseModel<List<TokenStatusModel>>
            {
                IsSuccess = true,
                Result = new List<TokenStatusModel> { new TokenStatusModel() }
            };

            _mockDal.Setup(x => x.GetStatuses(email, null)).ReturnsAsync(expected);

            // Act
            var result = await _bal.GetStatuses(email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.Single(result.Result);
        }

        // ========================
        // GET DROPDOWN
        // ========================
        [Fact]
        public async Task GetDropdown_Should_Return_Data_When_DAL_Returns_Data()
        {
            // Arrange
            var email = "test@test.com";
            var expected = new APIGetResponseModel<List<DropdownModel>>
            {
                IsSuccess = true,
                Result = new List<DropdownModel> { new DropdownModel() }
            };

            _mockDal.Setup(x => x.GetDropdown(email, null)).ReturnsAsync(expected);

            // Act
            var result = await _bal.GetDropdown(email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
            Assert.Single(result.Result);
        }

        [Fact]
        public async Task GetDropdown_Should_Return_Error_When_DAL_Throws_Exception()
        {
            // Arrange
            var email = "test@test.com";

            _mockDal.Setup(x => x.GetDropdown(email, null))
                    .ThrowsAsync(new Exception("DB Error"));

            // Act
            var result = await _bal.GetDropdown(email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Error while fetching dropdown", result.ErrorMsgs);
        }

        // ========================
        // NEXT TOKEN PREVIEW
        // ========================
        [Fact]
        public async Task NextTokenPreview_Should_Return_AccessDenied_When_Roles_Are_Empty()
        {
            // Arrange
            var request = new TokenRequestDto();
            var roles = new List<string>();
            var email = "test@test.com";

            // Act
            var result = await _bal.NextTokenPreview(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        [Fact]
        public async Task NextTokenPreview_Should_Return_Error_When_Request_Is_Null()
        {
            // Arrange
            TokenRequestDto request = null;
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            // Act
            var result = await _bal.NextTokenPreview(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid payload.", result.ErrorMsgs);
        }

        [Fact]
        public async Task NextTokenPreview_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new TokenRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            var expected = new APIGetResponseModel<TokenModel>
            {
                IsSuccess = true,
                Result = new TokenModel()
            };

            _mockDal.Setup(x => x.NextTokenPreview(request, roles, email, null)).ReturnsAsync(expected);

            // Act
            var result = await _bal.NextTokenPreview(request, roles, email);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Result);
        }

        [Fact]
        public async Task NextTokenPreview_Should_Return_Error_When_DAL_Throws_Exception()
        {
            // Arrange
            var request = new TokenRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@test.com";

            _mockDal.Setup(x => x.NextTokenPreview(request, roles, email, null))
                    .ThrowsAsync(new Exception("DB Error"));

            // Act
            var result = await _bal.NextTokenPreview(request, roles, email);

            // Assert
            Assert.False(result.IsSuccess);
            Assert.Contains("Error while previewing next token", result.ErrorMsgs);
        }
    }
}