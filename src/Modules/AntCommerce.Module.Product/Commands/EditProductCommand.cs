namespace AntCommerce.Module.Product.Commands
{
    using AntCommerce.Module.Product.DTOs;
    using MediatR;

    public class EditProductCommand : IRequest<ProductModel>
    {
        public ProductModel? Model { get; set; }
    }
}