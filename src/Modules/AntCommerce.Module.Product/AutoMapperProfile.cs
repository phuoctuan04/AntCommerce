using AntCommerce.Module.Product.DTOs;

namespace AntCommerce.Module.Product
{
    using AutoMapper;

    internal class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductModel, Contexts.Models.Product>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<Contexts.Models.Product, ProductModel>().ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<DTOs.ProductQueryModel, Commands.QueryProductCommand>().ReverseMap();
        }
    }
}