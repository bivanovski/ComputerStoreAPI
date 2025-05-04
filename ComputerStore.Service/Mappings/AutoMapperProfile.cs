using AutoMapper;
using ComputerStore.Data.Entities;
using ComputerStore.Service.DTOs;

namespace ComputerStore.Service.Mappings
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<Product, ProductDto>()
    .ForMember(dest => dest.Categories,
        opt => opt.MapFrom(src => src.Categories.Select(c => c.Name)));

            CreateMap<CreateProductDto, Product>();

        }
    }
}
