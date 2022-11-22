using AntCommerce.Module.GraphQL.DTOs;

namespace AntCommerce.Module.GraphQL
{
    using AutoMapper;

    internal class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductModel, Contexts.Models.Product>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Contexts.Models.Product, ProductModel>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}