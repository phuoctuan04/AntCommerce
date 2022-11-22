namespace AntCommerce.Module.GraphQL
{
    using AntCommerce.Module.GraphQL.DTOs;
    using AntCommerce.Module.GraphQL.Services;
    using HotChocolate.Subscriptions;

    public class Query
    {
        public async Task<IReadOnlyCollection<ProductModel>> GetAllProducts([Service] IProductQueryService productQueryService,       [Service] ITopicEventSender eventSender)
        {
            var products = await productQueryService.FindAllAsync();
            await eventSender.SendAsync("ReturnedProducts", products);
            return products;
        }
    }
}