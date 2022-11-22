namespace AntCommerce.Module.GraphQL.DTOs
{
    public class ProductModel
    {
        [GraphQLType(typeof(NonNullType<IdType>))]
        public int Id { get; set; }

        [GraphQLType(typeof(StringType))]
        public string? SKU { get; set; }

        [GraphQLType(typeof(StringType))]
        public string? Name { get; set; }

        [GraphQLType(typeof(StringType))]
        public string? Description { get; set; }

        [GraphQLType(typeof(DecimalType))]
        public decimal? Price { get; set; }
    }
}
