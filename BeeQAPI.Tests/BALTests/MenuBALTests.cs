using DAL.ContractIF;
using Models;
using Moq;
using Xunit;

namespace BeeQAPI.Tests.BALTests
{
    public class MenuBALTests
    {
        private readonly Mock<IDAL_Menu> _mockDal;
        private readonly BAL_Menu _bal;

        public MenuBALTests()
        {
            _mockDal = new Mock<IDAL_Menu>();
            _bal = new BAL_Menu(_mockDal.Object);
        }

        [Fact]
        public async Task GetSidebar_Should_Return_MenuList_When_DAL_Returns_Data()
        {
            // Arrange
            var email = "test@test.com";

            var menuList = new List<MenuModel>
            {
                new MenuModel(),
                new MenuModel()
            };

            _mockDal
                .Setup(x => x.GetSidebar(It.IsAny<string>()))
                .ReturnsAsync(menuList);

            // Act
            var result = await _bal.GetSidebar(email);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetSidebar_Should_Return_EmptyList_When_DAL_Returns_Null()
        {
            // Arrange
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.GetSidebar(It.IsAny<string>()))
                .ReturnsAsync((List<MenuModel>)null);

            // Act
            var result = await _bal.GetSidebar(email);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetSidebar_Should_Throw_Exception_When_DAL_Throws_Error()
        {
            // Arrange
            var email = "test@test.com";

            _mockDal
                .Setup(x => x.GetSidebar(It.IsAny<string>()))
                .ThrowsAsync(new Exception("DB error"));

            // Act + Assert
            var ex = await Assert.ThrowsAsync<Exception>(() => _bal.GetSidebar(email));

            Assert.Contains("BAL: Error processing sidebar", ex.Message);
        }
    }
}