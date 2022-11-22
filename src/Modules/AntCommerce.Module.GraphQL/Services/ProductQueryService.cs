namespace AntCommerce.Module.GraphQL.Services
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using AntCommerce.Module.Core;
    using AntCommerce.Module.GraphQL.Contexts;
    using AntCommerce.Module.GraphQL.DTOs;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class ProductQueryService : IProductQueryService
    {
        private readonly IMapper _mapper;
        private readonly DbContext _dbContext;
        private readonly ILogger _logger;

        public ProductQueryService(ProductDbContext productDbContext,
            ILogger<ProductQueryService> logger)
        {
            _dbContext = productDbContext;
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new AutoMapperProfile()); });
            _mapper = mappingConfig.CreateMapper();
            _logger = logger;
        }

        public async Task<IReadOnlyCollection<ProductModel>> FindAllAsync()
        {
            var data = await _dbContext.Set<Contexts.Models.Product>().ToListAsync();
            return _mapper.Map<List<ProductModel>>(data);
        }

        public async Task<ProductModel?> FindByIdAsync(int productId)
        {
            var product = await _dbContext.Set<Contexts.Models.Product>()
                .FirstOrDefaultAsync(product => product.Id == productId);
            if (product is not null)
            {
                return _mapper.Map<ProductModel>(product);
            }

            return null;
        }

        public async Task<PagingResult<ProductModel>> SearchAsync(ProductQueryModel productQueryModel,
            CancellationToken cancellationToken)
        {
            var query = _dbContext.Set<Contexts.Models.Product>();

            var data = await query
                .Where(productQueryModel.FilterProductName())
                .Where(productQueryModel.FilterProductSku())
                .Where(productQueryModel.FilterProductFromPrice())
                .Where(productQueryModel.FilterProductToPrice())
                .Skip((productQueryModel.Page - 1) * productQueryModel.Limit)
                .Take(productQueryModel.Limit)
                .ToListAsync();
            var productModels = _mapper.Map<List<ProductModel>>(data);
            var total = await query.CountAsync();

            return new PagingResult<ProductModel>()
            {
                Data = productModels,
                MetaData = new MetaData()
                {
                    Total = total,
                    Page = productQueryModel.Page,
                    Limit = productQueryModel.Limit,
                }
            };
        }
    }
}
