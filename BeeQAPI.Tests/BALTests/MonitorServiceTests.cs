using BAL.Services;
using System.Threading.Tasks;
using Xunit;

namespace BeeQAPI.Tests.BALTests
{
    public class MonitorServiceTests
    {
        private readonly MonitorService _service;

        public MonitorServiceTests()
        {
            _service = new MonitorService();
        }

        [Fact]
        public async Task GetBranchByKey_Should_Return_1()
        {
            // Arrange
            var monitorKey = "ABC123";

            // Act
            var result = await _service.GetBranchByKey(monitorKey);

            // Assert
            Assert.Equal("1", result);
        }
    }
}