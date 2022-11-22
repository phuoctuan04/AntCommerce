namespace AntCommerce.Module.GraphQL.Services
{
    using System;
    using System.Threading.Tasks;
    using AntCommerce.Module.GraphQL.Contexts;
    using AntCommerce.Module.GraphQL.DTOs;
    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;

    public class ProductCommandService : IProductCommandService
    {
        private readonly IMapper _mapper;
        private readonly DbContext _dbContext;
        private readonly ILogger _logger;
        public ProductCommandService(ProductDbContext productDbContext,
            ILogger<ProductCommandService> logger)
        {
            _dbContext = productDbContext;
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new AutoMapperProfile()); });
            _mapper = mappingConfig.CreateMapper();
            _logger = logger;
        }

        public async Task<ProductModel> CreateAsync(ProductModel productModel)
        {
            var product = _mapper.Map<Contexts.Models.Product>(productModel);
            await _dbContext.Set<Contexts.Models.Product>().AddAsync(product);
            await _dbContext.SaveChangesAsync();

            productModel.Id = product.Id;
            return productModel;
        }

        public async Task<int> DeleteAsync(int productId)
        {
            var product = await _dbContext.Set<Contexts.Models.Product>()
                .FirstOrDefaultAsync(product => product.Id == productId);
            if (product is not null)
            {
                _dbContext.Remove(product);
                return await _dbContext.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<ProductModel> UpdateAsync(ProductModel productModel)
        {
            var product = await _dbContext.Set<Contexts.Models.Product>()
                .FirstOrDefaultAsync(product => product.Id == productModel.Id);
            if (product is not null)
            {
                var mapToUpdate = _mapper.Map<ProductModel, Contexts.Models.Product>(productModel, product);
                mapToUpdate.UpdateAt = DateTime.UtcNow;
                _dbContext.Update(mapToUpdate);
                await _dbContext.SaveChangesAsync();
                return productModel;
            }

            return productModel;
        }
    }
}
