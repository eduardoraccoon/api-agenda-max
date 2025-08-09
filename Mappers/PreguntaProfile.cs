using AutoMapper;
using api_iso_med_pg.Models;
using api_iso_med_pg.DTOs.Pregunta;

namespace api_iso_med_pg.Mappers;

public class PreguntaProfile : Profile
{
    public PreguntaProfile()
    {
        CreateMap<Pregunta, PreguntaDto>().ReverseMap();
        CreateMap<CreatePreguntaDto, Pregunta>()
            .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.Label))
            .ForMember(dest => dest.TipoId, opt => opt.MapFrom(src => src.TipoId));
        CreateMap<UpdatePreguntaDto, Pregunta>()
            .ForMember(dest => dest.Label, opt => opt.MapFrom(src => src.Label))
            .ForMember(dest => dest.TipoId, opt => opt.MapFrom(src => src.TipoId));
    }
}
