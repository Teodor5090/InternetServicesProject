using AutoMapper;
using Store.Application.DTOs;
using Store.Domain.Entities;

namespace Store.Application
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.ProductIds, opt => opt.MapFrom(src => src.ProductCategories.Select(pc => pc.ProductId)))
                .ReverseMap()
                .ForMember(dest => dest.ProductCategories, opt => opt.Ignore());

            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.CategoryIds, opt => opt.MapFrom(src => src.ProductCategories.Select(pc => pc.CategoryId)))
                .ReverseMap()
                .ForMember(dest => dest.ProductCategories, opt => opt.Ignore());
        }
    }
}
