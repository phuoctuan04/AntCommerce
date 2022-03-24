namespace AntCommerce.Module.Product.Events
{
    using AntCommerce.Module.Core;
    using AntCommerce.Module.Product.Commands;
    using AntCommerce.Module.Product.DTOs;
    using AntCommerce.Module.Product.Services;
    using AutoMapper;
    using MediatR;

    public class QueryProductHandler : IRequestHandler<QueryProductCommand, PagingResult<ProductModel>>
    {
        private readonly IProductQueryService _productQueryService;

        public QueryProductHandler(IProductQueryService productQueryService)
        {
            this._productQueryService = productQueryService;
        }

        public async Task<PagingResult<ProductModel>> Handle(QueryProductCommand request, CancellationToken cancellationToken)
        {
            return await _productQueryService.SearchAsync(request.Model, cancellationToken);
        }
    }
}