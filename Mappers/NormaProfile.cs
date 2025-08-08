using AutoMapper;
using api_iso_med_pg.Models;
using api_iso_med_pg.DTOs;

namespace api_iso_med_pg.Mappers
{
    public class NormaProfile : Profile
    {
        public NormaProfile()
        {
            CreateMap<Norma, NormaDto>().ReverseMap();
            CreateMap<CreateNormaDto, Norma>()
                .ForMember(dest => dest.CreadoId, opt => opt.MapFrom(src => src.CreadoId))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.FechaCreacion));
            CreateMap<UpdateNormaDto, Norma>()
                .ForMember(dest => dest.ActualizadoId, opt => opt.MapFrom(src => src.ActualizadoId))
                .ForMember(dest => dest.FechaActualizacion, opt => opt.MapFrom(src => src.FechaActualizacion));
        }
    }
}
