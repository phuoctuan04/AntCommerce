namespace AntCommerce.Module.Product.Events
{
    using AntCommerce.Module.Product.Commands;
    using AntCommerce.Module.Product.DTOs;
    using AntCommerce.Module.Product.Services;
    using MediatR;

    public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductModel>
    {
        private readonly IProductCommandService _productCommandService;

        public CreateProductHandler(IProductCommandService productCommandService)
        {
            this._productCommandService = productCommandService;
        }

        public async Task<ProductModel> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            return await _productCommandService.CreateAsync(request.Model);
        }
    }
}