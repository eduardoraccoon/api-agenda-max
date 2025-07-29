using AutoMapper;
using api_iso_med_pg.Models;
using api_iso_med_pg.DTOs;

namespace api_iso_med_pg.Mappers
{
    public class NormProfile : Profile
    {
        public NormProfile()
        {
            CreateMap<Norma, NormaDto>().ReverseMap();
            CreateMap<CreateNormaDto, Norma>()
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
            CreateMap<UpdateNormaDto, Norma>()
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
        }
    }
}
