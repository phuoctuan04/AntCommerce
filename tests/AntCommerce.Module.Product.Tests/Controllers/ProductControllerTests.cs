namespace AntCommerce.Module.Product.Tests.Controllers
{
    using System.Threading.Tasks;
    using AntCommerce.Module.Core;
    using AntCommerce.Module.Product.Controllers;
    using AntCommerce.Module.Product.DTOs;
    using AntCommerce.Module.Product.Services;
    using MediatR;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using NSubstitute;
    using Xunit;
    using AntCommerce.Module.Core.Cache;
    using Microsoft.Extensions.Caching.Distributed;

    public class ProductControllerTests
    {
        private readonly ProductController _controller;
        private readonly IProductQueryService _productQueryService;
        private readonly IProductCommandService _productCommandService;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<ProductController> _logger;

        private readonly IDistributedCache _cache;

        public ProductControllerTests()
        {
            _productQueryService = Substitute.For<IProductQueryService>();
            _productCommandService = Substitute.For<IProductCommandService>();
            _mediator = Substitute.For<IMediator>();
            _httpContextAccessor = Substitute.For<HttpContextAccessor>();
            _logger = Substitute.For<ILogger<ProductController>>();
            _cache = Substitute.For<IDistributedCache>();
            var context = new DefaultHttpContext
            {
                Connection =
                {
                    Id = Guid.NewGuid().ToString()
                }
            };
            _httpContextAccessor = new HttpContextAccessor()
            {
                HttpContext = context
            };
            _controller = new ProductController(_productQueryService, _productCommandService, _mediator,
                _httpContextAccessor, _logger, _cache)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = context
                }
            };
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnSuccess()
        {
            var productModesl = new List<ProductModel>
            {
                new ProductModel() { SKU = "111", Name = "IPhone", Price = 666, Id = 1 },
            };

            _cache.GetAsync<IReadOnlyCollection<ProductModel>>(Arg.Compat.Any<string>(), Arg.Compat.Any<CancellationToken>()).Returns(x=>productModesl);
            _cache.SetAsync(Arg.Compat.Any<string>(), Arg.Compat.Any<byte[]>(), Arg.Compat.Any<DistributedCacheEntryOptions>());
            _productQueryService.FindAllAsync().Returns(productModesl);
            var actual = await _controller.Get();
            Assert.IsType<OkObjectResult>(actual);

            var okObjectResult = actual as OkObjectResult;
            var response = okObjectResult.Value as List<ProductModel>;
            Assert.Equal(1, response.Count);
        }

        [Fact]
        public async Task GetProductById_WhenCalled_ReturnSuccess()
        {
            var productModel = new ProductModel() { SKU = "111", Name = "IPhone", Price = 666, Id = 1 };
            _productQueryService.FindByIdAsync(Arg.Compat.Any<int>()).Returns(productModel);
            var actual = await _controller.GetById(1);
            Assert.IsType<OkObjectResult>(actual);

            var okObjectResult = actual as OkObjectResult;
            var response = okObjectResult.Value as ProductModel;
            Assert.Equal("111", response.SKU);
        }

        [Fact]
        public async Task GetProductById_WhenCalled_ReturnNotFound()
        {
            ProductModel productModel = null;
            _productQueryService.FindByIdAsync(Arg.Compat.Any<int>()).Returns(productModel);
            var actual = await _controller.GetById(1);
            Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public async Task Search_WhenCalled_ReturnSuccess()
        {
            var productModesl = new List<ProductModel>
            {
                new ProductModel() { SKU = "111", Name = "IPhone", Price = 666, Id = 1 },
            };

            var mockData = new PagingResult<ProductModel>()
            {
                Data = productModesl,
                MetaData = new MetaData()
                {
                    Total = 1,
                    Page = 25,
                    Limit = 25
                }
            };

            _productQueryService.SearchAsync(Arg.Compat.Any<ProductQueryModel>(), Arg.Compat.Any<CancellationToken>()).Returns(mockData);
            var actual = await _controller.Search(new ProductQueryModel()
            {
                Page = 1,
                Limit = 25,
            });
            Assert.IsType<OkObjectResult>(actual);
        }

        [Fact]
        public async Task Post_WhenCalled_ReturnBadRequest()
        {


            var actual = await _controller.Post(new ProductModel()
            {
                SKU = "111",
            });
            Assert.IsType<BadRequestObjectResult>(actual);
        }

        [Fact]
        public async Task Post_WhenCalled_ReturnCreated()
        {
            var productModel = new ProductModel()
            {
                SKU = "11111",
                Name = "Demo",
                Description = "",
                Price = 10
            };
            _productCommandService.CreateAsync(Arg.Compat.Any<ProductModel>()).Returns(productModel);
            var actual = await _controller.Post(new ProductModel()
            {
                SKU = "11111",
                Name = "Demo",
                Description = "",
                Price = 10
            });
            Assert.IsType<CreatedResult>(actual);
        }

        [Fact]
        public async Task Put_WhenCalled_ReturnNotFound()
        {
            ProductModel productModel = null;
            _productQueryService.FindByIdAsync(Arg.Compat.Any<int>()).Returns(productModel);
            var actual = await _controller.Put(1, new ProductModel()
            {
                SKU = "111",
            });
            Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public async Task Put_WhenCalled_ReturnBadRequest()
        {
            var productModel = new ProductModel() { SKU = "111", Name = "IPhone", Price = 666, Id = 1 };
            _productQueryService.FindByIdAsync(Arg.Compat.Any<int>()).Returns(productModel);

            var actual = await _controller.Put(1, new ProductModel()
            {
                SKU = "111",
            });
            Assert.IsType<BadRequestObjectResult>(actual);
        }

        [Fact]
        public async Task Put_WhenCalled_ReturnSuccess()
        {
            var productModel = new ProductModel()
            {
                SKU = "11111",
                Name = "Demo",
                Description = "",
                Price = 10
            };
            var productModelOld = new ProductModel() { SKU = "111", Name = "IPhone", Price = 666, Id = 1 };
            _productQueryService.FindByIdAsync(Arg.Compat.Any<int>()).Returns(productModelOld);

            _productCommandService.UpdateAsync(Arg.Compat.Any<ProductModel>()).Returns(productModel);
            var actual = await _controller.Put(1, new ProductModel()
            {
                SKU = "11111",
                Name = "Demo",
                Description = "",
                Price = 10
            });
            Assert.IsType<OkObjectResult>(actual);
        }


        [Fact]
        public async Task Delete_WhenCalled_ReturnNotFound()
        {
            ProductModel productModel = null;
            _productQueryService.FindByIdAsync(Arg.Compat.Any<int>()).Returns(productModel);
            var actual = await _controller.Delete(1);
            Assert.IsType<NotFoundResult>(actual);
        }

        [Fact]
        public async Task Delete_WhenCalled_ReturnNoContent()
        {
            var productModel = new ProductModel() { SKU = "111", Name = "IPhone", Price = 666, Id = 1 };
            _productQueryService.FindByIdAsync(Arg.Compat.Any<int>()).Returns(productModel);
            _productCommandService.DeleteAsync(Arg.Compat.Any<int>()).Returns(1);
            var actual = await _controller.Delete(1);
            Assert.IsType<NoContentResult>(actual);
        }
    }
}
