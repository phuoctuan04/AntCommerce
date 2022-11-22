using AntCommerce.Module.GraphQL.DTOs;
using AntCommerce.Module.GraphQL.Services;
using HotChocolate;
using HotChocolate.Subscriptions;

namespace AntCommerce.Module.GraphQL
{
    public class Mutation
    {
        public async Task<ProductModel> CreateProduct([Service] IProductCommandService productCommandService, [Service] ITopicEventSender eventSender, ProductModel productModel)
        {
            var result = await productCommandService.CreateAsync(productModel);
            await eventSender.SendAsync("AuthorCreated", result);
            return result;
        }
    }
}
