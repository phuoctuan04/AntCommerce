namespace AntCommerce.Module.Product.Events
{
    using AntCommerce.Module.Product.Commands;
    using AntCommerce.Module.Product.DTOs;
    using AntCommerce.Module.Product.Services;
    using MediatR;

    public class EditProductHandler : IRequestHandler<EditProductCommand, ProductModel>
    {
        private readonly IProductCommandService _productCommandService;

        public EditProductHandler(IProductCommandService productCommandService)
        {
            this._productCommandService = productCommandService;
        }

        public async Task<ProductModel> Handle(EditProductCommand request, CancellationToken cancellationToken)
        {
            return await _productCommandService.UpdateAsync(request.Model);
        }
    }
}