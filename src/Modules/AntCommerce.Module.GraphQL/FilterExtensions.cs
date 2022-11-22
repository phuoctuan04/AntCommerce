namespace AntCommerce.Module.GraphQL
{
    using System.Linq.Expressions;
    using AntCommerce.Module.Product.DTOs;

    public static class FilterExtensions
    {
        public static Expression<Func<Contexts.Models.Product, bool>> FilterProductName(
            this ProductQueryModel productQueryModel)
        {
            if (!string.IsNullOrEmpty(productQueryModel.Name))
            {
                return product =>
                    product.Name.Contains(productQueryModel.Name) ||
                    (product.Description != null && product.Description.Contains(productQueryModel.Name));
            }

            return product => true;
        }

        public static Expression<Func<Contexts.Models.Product, bool>> FilterProductSku(
            this ProductQueryModel productQueryModel)
        {
            if (!string.IsNullOrEmpty(productQueryModel.Sku))
            {
                return product => product.SKU.Contains(productQueryModel.Sku);
            }

            return product => true;
        }

        public static Expression<Func<Contexts.Models.Product, bool>> FilterProductFromPrice(
            this ProductQueryModel productQueryModel)
        {
            if (productQueryModel.FromPrice.HasValue)
            {
                return product => product.Price >= productQueryModel.FromPrice;
            }

            return product => true;
        }

        public static Expression<Func<Contexts.Models.Product, bool>> FilterProductToPrice(
            this ProductQueryModel productQueryModel)
        {
            if (productQueryModel.ToPrice.HasValue)
            {
                return product => product.Price <= productQueryModel.ToPrice;
            }

            return product => true;
        }
    }
}
