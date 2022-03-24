namespace AntCommerce.Module.Product.Commands
{
    using AntCommerce.Module.Product.DTOs;
    using MediatR;

    public class CreateProductCommand : IRequest<ProductModel>
    {
        public ProductModel Model { get; set; }
    }
}