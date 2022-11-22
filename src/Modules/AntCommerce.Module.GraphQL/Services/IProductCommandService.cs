namespace AntCommerce.Module.GraphQL.Services
{
    using System.Threading.Tasks;
    using AntCommerce.Module.GraphQL.DTOs;

    public interface IProductCommandService
    {
        Task<ProductModel> CreateAsync(ProductModel productModel);

        Task<ProductModel> UpdateAsync(ProductModel productModel);

        Task<int> DeleteAsync(int productId);
    }
}