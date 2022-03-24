namespace AntCommerce.Module.Product.Commands
{
    using AntCommerce.Module.Core;
    using AntCommerce.Module.Product.DTOs;
    using MediatR;

    public class QueryProductCommand : IRequest<PagingResult<ProductModel>>
    {
        public ProductQueryModel Model { get; set; }
    }
}