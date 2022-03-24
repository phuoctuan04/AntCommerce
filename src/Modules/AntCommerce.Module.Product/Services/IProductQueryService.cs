namespace AntCommerce.Module.Product.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AntCommerce.Module.Core;
    using AntCommerce.Module.Product.DTOs;

    public interface IProductQueryService
    {
        Task<IReadOnlyCollection<ProductModel>> FindAllAsync();

        Task<ProductModel?> FindByIdAsync(int productId);

        Task<PagingResult<ProductModel>> SearchAsync(ProductQueryModel productQueryModel, CancellationToken cancellationToken);
    }
}
