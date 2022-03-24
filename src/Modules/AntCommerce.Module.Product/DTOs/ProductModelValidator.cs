namespace AntCommerce.Module.Product.DTOs
{
    using FluentValidation;

    public class ProductModelValidator : AbstractValidator<ProductModel>
    {
        public ProductModelValidator()
        {
            RuleFor(x => x.SKU).NotEmpty().WithMessage("Product Sku cannot be empty");
            RuleFor(x => x.SKU).Length(5).WithMessage("Product Sku must be length 5 character");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Product name cannot be empty");
            RuleFor(x => x.Name).MinimumLength(3).WithMessage("Product name must be length more than 3 character");
            RuleFor(x => x.Price).GreaterThan(0);
        }
    }
}