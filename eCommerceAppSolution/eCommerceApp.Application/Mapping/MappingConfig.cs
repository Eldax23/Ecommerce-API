using AutoMapper;
using eCommerceApp.Application.DTOs.Category;
using eCommerceApp.Application.DTOs.Product;
using eCommerceApp.Domain.Entites;

namespace eCommerceApp.Application.Mapping;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<CreateCategory, Category>();
        CreateMap<CreateProduct, Product>();
        CreateMap<Category, CreateCategory>();
        CreateMap<Product, GetProduct>();

        CreateMap<GetCategory, Category>();
        CreateMap<Category, GetCategory>();
        CreateMap<GetProduct, Product>();
        CreateMap<Product , UpdateProduct>();
        CreateMap<UpdateProduct , Product>();

    }
}