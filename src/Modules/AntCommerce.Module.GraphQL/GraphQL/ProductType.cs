using AntCommerce.Module.GraphQL.DTOs;
using HotChocolate.Types;

namespace AntCommerce.Module.GraphQL
{
    public class ProductType : ObjectType<ProductModel>
    {
        protected override void Configure(IObjectTypeDescriptor<ProductModel> descriptor)
        {
            descriptor.Field(a => a.Id).Type<IdType>();
            descriptor.Field(a => a.SKU).Type<StringType>();
            descriptor.Field(a => a.Name).Type<StringType>();
            descriptor.Field(a => a.Description).Type<StringType>();
            descriptor.Field(a => a.Price).Type<DecimalType>();
            //descriptor.Field<BlogPostResolver>(b =>
            //b.GetBlogPosts(default, default));
        }
    }
}
