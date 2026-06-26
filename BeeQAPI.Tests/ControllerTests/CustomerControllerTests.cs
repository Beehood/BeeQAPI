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
    public class CustomerControllerTests
    {
        private readonly Mock<IBAL_Customer> _balMock;
        private readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _balMock = new Mock<IBAL_Customer>();
            _controller = new CustomerController(_balMock.Object);
        }

        // =========================================================
        // Helper: Set User Claims
        // =========================================================
        private void SetUserClaims(List<string> roles, string? email)
        {
            var claims = new List<Claim>();

            if (!string.IsNullOrWhiteSpace(email))
                claims.Add(new Claim(ClaimTypes.NameIdentifier, email));

            if (roles != null && roles.Any())
                claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

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

        // =========================================================
        // GET ALL
        // =========================================================
        [Fact]
        public async Task GetAll_Should_Return_Customer_List_Successfully()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var roles = new List<string> { "Super Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<List<CustomerModel>>
            {
                IsSuccess = true,
                Result = new List<CustomerModel>
                {
                    new CustomerModel()
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
        }

        [Fact]
        public async Task GetAll_Should_Return_Failure_When_BAL_Returns_Error()
        {
            // Arrange
            var request = new PaginationRequestDto();
            var roles = new List<string> { "Branch Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<List<CustomerModel>>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Access denied." }
            };

            _balMock
                .Setup(x => x.GetAll(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetAll(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }

        // =========================================================
        // GET BY ID
        // =========================================================
        [Fact]
        public async Task GetById_Should_Return_Customer_By_Id()
        {
            // Arrange
            long id = 1;
            var roles = new List<string> { "Super Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<CustomerModel>
            {
                IsSuccess = true,
                Result = new CustomerModel()
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
        }

        [Fact]
        public async Task GetById_Should_Return_Failure_When_BAL_Returns_Error()
        {
            // Arrange
            long id = 999;
            var roles = new List<string> { "Org Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<CustomerModel>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Customer not found." }
            };

            _balMock
                .Setup(x => x.GetById(id, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Customer not found.", result.ErrorMsgs);
        }

        // =========================================================
        // CREATE
        // =========================================================
        [Fact]
        public async Task Create_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new CustomerRequestDto
            {
                Name = "Test Customer"
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
        }

        [Fact]
        public async Task Create_Should_Return_Failure_When_BAL_Returns_Error()
        {
            // Arrange
            var request = new CustomerRequestDto();

            var roles = new List<string> { "Branch Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Customer Name is required" }
            };

            _balMock
                .Setup(x => x.Create(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Create(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Customer Name is required", result.ErrorMsgs);
        }

        // =========================================================
        // UPDATE
        // =========================================================
        [Fact]
        public async Task Update_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new CustomerRequestDto
            {
                CustomerId = 1,
                Name = "Updated Customer"
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
        }

        [Fact]
        public async Task Update_Should_Return_Failure_When_BAL_Returns_Error()
        {
            // Arrange
            var request = new CustomerRequestDto
            {
                CustomerId = 0
            };

            var roles = new List<string> { "Org Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Invalid customer data." }
            };

            _balMock
                .Setup(x => x.Update(request, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Update(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid customer data.", result.ErrorMsgs);
        }

        // =========================================================
        // CHANGE STATUS
        // =========================================================
        [Fact]
        public async Task ChangeStatus_Should_Return_Success_When_Request_Is_Valid()
        {
            // Arrange
            var request = new CustomerStatusRequestDto
            {
                CustomerId = 1
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
                .Setup(x => x.ChangeStatus(request.CustomerId, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.ChangeStatus(request);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(1, result.Result);
        }

        [Fact]
        public async Task ChangeStatus_Should_Return_Failure_When_BAL_Returns_Error()
        {
            // Arrange
            var request = new CustomerStatusRequestDto
            {
                CustomerId = 0
            };

            var roles = new List<string> { "Branch Admin" };
            var email = "test@mail.com";

            SetUserClaims(roles, email);

            var expectedResponse = new APIGetResponseModel<int>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "Invalid customer ID." }
            };

            _balMock
                .Setup(x => x.ChangeStatus(request.CustomerId, roles, email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.ChangeStatus(request);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("Invalid customer ID.", result.ErrorMsgs);
        }

        // =========================================================
        // DROPDOWN
        // =========================================================
        [Fact]
        public async Task GetDropdown_Should_Return_Dropdown_List_Successfully()
        {
            // Arrange
            var email = "test@mail.com";

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
        }

        [Fact]
        public async Task GetDropdown_Should_Return_Failure_When_BAL_Returns_Error()
        {
            // Arrange
            var email = "test@mail.com";

            SetUserClaims(new List<string> { "Super Admin" }, email);

            var expectedResponse = new APIGetResponseModel<List<DropdownModel>>
            {
                IsSuccess = false,
                ErrorMsgs = new List<string> { "No dropdown data found." }
            };

            _balMock
                .Setup(x => x.GetDropdown(email, null))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetDropdown();

            // Assert
            Assert.NotNull(result);
            Assert.False(result.IsSuccess);
            Assert.Contains("No dropdown data found.", result.ErrorMsgs);
        }
    }
}