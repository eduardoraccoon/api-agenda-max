using api_iso_med_pg.DTOs;
using api_iso_med_pg.Models;
using AutoMapper;

namespace api_iso_med_pg.Mappers;

public class EntrevistaProfile : Profile
{
    public EntrevistaProfile()
    {
        CreateMap<Entrevista, EntrevistaDto>().ReverseMap();
        CreateMap<CreateEntrevistaDto, Entrevista>()
            .ForMember(dest => dest.CreadoId, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.CreatedAt));
        CreateMap<UpdateEntrevistaDto, Entrevista>()
            .ForMember(dest => dest.ActualizadoId, opt => opt.MapFrom(src => src.UpdatedBy))
            .ForMember(dest => dest.FechaActualizacion, opt => opt.MapFrom(src => src.UpdatedAt));
    }
}
