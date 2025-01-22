using AutoMapper;
using eCommerceApp.Application.DTOs.Cart;
using eCommerceApp.Application.DTOs.Category;
using eCommerceApp.Application.DTOs.Identity;
using eCommerceApp.Application.DTOs.Product;
using eCommerceApp.Domain.Entites;
using eCommerceApp.Domain.Entites.Cart;
using eCommerceApp.Domain.Entites.Identity;

namespace eCommerceApp.Application.Mapping;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<CreateCategory, Category>();
        CreateMap<CreateProduct, Product>();
        CreateMap<Category, CreateCategory>();
        CreateMap<Product, GetProduct>();
        CreateMap<AppUser, CreateUser>();
        CreateMap<CreateUser, AppUser>();
        CreateMap<LoginUser, AppUser>();

        CreateMap<GetCategory, Category>();
        CreateMap<Category, GetCategory>();
        CreateMap<GetProduct, Product>();
        CreateMap<Product , UpdateProduct>();
        CreateMap<UpdateProduct , Product>();
        CreateMap<CreateArchive , Archive>();

        CreateMap<PaymentMethod, GetPaymentMethod>();

    }
}