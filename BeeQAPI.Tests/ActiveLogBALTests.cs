using BAL.Services;
using DAL.ContractIF;
using Models;
using Moq;
using System.Data;
using Xunit;

namespace BeeQAPI.Tests
{
    public class ActiveLogBALTests
    {
        private readonly Mock<IDAL_ActiveLog> _mockDal;
        private readonly BAL_ActiveLog _bal;

        public ActiveLogBALTests()
        {
            _mockDal = new Mock<IDAL_ActiveLog>();
            _bal = new BAL_ActiveLog(_mockDal.Object);
        }

        [Fact]
        public async Task GetAll_Should_Return_AccessDenied_When_Role_Is_Invalid()
        {
            var request = new PaginationRequestDto();

            var roles = new List<string>
    {
        "User"
    };

            var result = await _bal.GetAll(
                request,
                roles,
                "test@test.com");

            Assert.False(result.IsSuccess);
            Assert.Contains("Access denied.", result.ErrorMsgs);
        }
    }
}