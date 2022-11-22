namespace AntCommerce.Module.Product.Tests.Services
{
    using AntCommerce.Module.Product.Contexts;
    using AntCommerce.Module.Product.DTOs;
    using AntCommerce.Module.Product.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using MockQueryable.FakeItEasy;
    using NSubstitute;
    using Xunit;
    using DbProduct = AntCommerce.Module.Product.Contexts.Models.Product;

    public class ProductCommandServiceTests
    {
        private readonly ProductDbContext _dbContext;
        private readonly IProductCommandService _productCommandService;
        private readonly ILogger<ProductCommandService> _logger;

        private readonly List<DbProduct> productData = new List<DbProduct>
        {
            new DbProduct() { SKU = "111", Name = "IPhone", Price = 666, Id = 1 },
            new DbProduct() { SKU = "112", Name = "IPhone", Price = 777, Id = 2 },
            new DbProduct() { SKU = "113", Name = "Sam sung", Price = 888, Id = 3 },
            new DbProduct() { SKU = "113", Name = "Sam sung", Price = 888, Id = 4 },
        };

        public ProductCommandServiceTests()
        {
            _logger = Substitute.For<ILogger<ProductCommandService>>();
            var options = new DbContextOptionsBuilder<ProductDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = Substitute.For<ProductDbContext>(options);

            var mockDbSet = productData.AsQueryable().BuildMockDbSet();
            _dbContext.Set<DbProduct>().Returns(mockDbSet);

            _productCommandService = new ProductCommandService(_dbContext, _logger);
        }

        [Fact]
        public async Task CreateAsync_WhenCalled_ReturnProductModel()
        {
            // Arrange
            var mockModel = new ProductModel() { SKU = "111XA", Name = "IPhone", Price = 666, Id = 1 };

            // Actual
            var actual = await _productCommandService.CreateAsync(mockModel);

            // Assert

            Assert.Equal(1, actual.Id);
        }

        [Fact]
        public async Task UpdateAsync_WhenCalled_ReturnProductModel()
        {
            // Arrange
            var mockModel = new ProductModel() { SKU = "111", Name = "IPhone_new", Price = 666, Id = 1 };

            // Actual
            var actual = await _productCommandService.UpdateAsync(mockModel);

            // Assert

            Assert.Equal(1, actual.Id);
            Assert.Equal("IPhone_new", actual.Name);
        }

        [Fact]
        public async Task DeleteAsync_WhenCalled_Return_NumOfRecord_Effect()
        {
            // Arrange
            var productId = 1;

            // Actual
            _dbContext.SaveChangesAsync().Returns(1);
            var actual = await _productCommandService.DeleteAsync(productId);

            // Assert

            Assert.Equal(1, actual);
        }

        [Fact]
        public async Task DeleteAsync_WhenCalled_Return_Fail_IfProduct_NotFound()
        {
            // Arrange
            var productId = 999;

            // Actual
            var actual = await _productCommandService.DeleteAsync(productId);

            // Assert

            Assert.Equal(0, actual);
        }
    }
}