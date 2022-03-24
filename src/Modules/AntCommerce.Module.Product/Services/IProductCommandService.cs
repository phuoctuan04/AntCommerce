namespace AntCommerce.Module.Product.Services
{
    using System.Threading.Tasks;
    using AntCommerce.Module.Product.DTOs;

    public interface IProductCommandService
    {
        Task<ProductModel> CreateAsync(ProductModel productModel);

        Task<ProductModel> UpdateAsync(ProductModel productModel);

        Task<int> DeleteAsync(int productId);
    }
}