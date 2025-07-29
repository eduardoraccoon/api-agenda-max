using AutoMapper;
using api_iso_med_pg.DTOs;
using api_iso_med_pg.Models;

namespace api_iso_med_pg.Mappers
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
        }
    }
}
