using AntCommerce.Module.GraphQL.DTOs;
using AntCommerce.Module.GraphQL.Services;
using HotChocolate;
using HotChocolate.Resolvers;

namespace AntCommerce.Module.GraphQL
{
    public class ProductResolver
    {
        private readonly IProductQueryService _productQueryService;

        public ProductResolver([Service] IProductQueryService productQueryService)
        {
            _productQueryService = productQueryService;
        }

        public async Task<IEnumerable<ProductModel>> FindAllAsync(IResolverContext ctx)
        {
            return await _productQueryService.FindAllAsync();
        }
    }
}