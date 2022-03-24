namespace AntCommerce.Module.Product.Tests.Services
{
    using AntCommerce.Module.Core;
    using AntCommerce.Module.Product.Contexts;
    using AntCommerce.Module.Product.DTOs;
    using AntCommerce.Module.Product.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using MockQueryable.NSubstitute;
    using NSubstitute;
    using Xunit;
    using DbProduct = AntCommerce.Module.Product.Contexts.Models.Product;

    public class ProductQueryServiceTests
    {
        private readonly ProductDbContext _dbContext;
        private readonly IProductQueryService _productQueryService;
        private readonly ILogger<ProductQueryService> _logger;

        private readonly List<DbProduct> productData = new List<DbProduct>
        {
            new DbProduct() { SKU = "111", Name = "IPhone", Price = 666, Id = 1 },
            new DbProduct() { SKU = "112", Name = "IPhone", Price = 777, Id = 2 },
            new DbProduct() { SKU = "113", Name = "Sam sung", Price = 888, Id = 3 },
            new DbProduct() { SKU = "113", Name = "Sam sung", Price = 888, Id = 4 },
        };

        public ProductQueryServiceTests()
        {
            _logger = Substitute.For<ILogger<ProductQueryService>>();
            var options = new DbContextOptionsBuilder<ProductDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _dbContext = Substitute.For<ProductDbContext>(options);

            var mockDbSet = productData.AsQueryable().BuildMockDbSet();
            _dbContext.Set<DbProduct>().Returns(mockDbSet);

            _productQueryService = new ProductQueryService(_dbContext, _logger);
        }

        [Fact]
        public async Task FindAllAsync_WhenCalled_ReturnListProductModel()
        {
            // Arrange

            // Actual
            var actual = await _productQueryService.FindAllAsync();

            // Assert
            Assert.Equal(4, actual.Count);
        }

        [Fact]
        public async Task FindByIdAsync_WhenCalled_ReturnSingleProductModel()
        {
            // Arrange
            var productId = 1;
            // Actual
            var actual = await _productQueryService.FindByIdAsync(productId);

            // Assert
            var expected = new ProductModel() { SKU = "111", Name = "IPhone", Price = 666, Id = 1 };
            Assert.NotNull(actual);
            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.SKU, actual.SKU);
            Assert.Equal(expected.Price, actual.Price);
        }

        [Fact]
        public async Task FindByIdAsync_WhenCalled_Return_Null()
        {
            // Arrange
            var productId = 9999;
            // Actual
            var actual = await _productQueryService.FindByIdAsync(productId);

            // Assert
            Assert.Null(actual);
        }

        [Fact]
        public async Task SearchAsync_WhenCalled_ReturnPagingResult_ProductModel()
        {
            // Arrange
            var productQueryModel = new ProductQueryModel()
            {
                Page = 1,
                Limit = 2,
                Name = "IPhone"
            };
            
            // Actual
            var actual = await _productQueryService.SearchAsync(productQueryModel, new CancellationToken());

            // Assert
            var expected = new PagingResult<ProductModel>()
            {
                Data = new List<ProductModel>()
                {
                    new ProductModel() { SKU = "111", Name = "IPhone", Price = 666, Id = 1 },
                    new ProductModel() { SKU = "222", Name = "IPhone", Price = 666, Id = 2 },
                },
                MetaData = new MetaData()
                {
                    Total = 4,
                    Page = 1,
                    Limit = 2
                }
            };

            Assert.Equal(expected.Data.Count, actual.Data.Count);
            Assert.Equal(expected.MetaData.Total, actual.MetaData.Total);
        }
    }
}
