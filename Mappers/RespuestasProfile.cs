using AutoMapper;
using api_iso_med_pg.Models;
using api_iso_med_pg.DTOs.Respuesta;

namespace api_iso_med_pg.Mappers;

public class RespuestaProfile : Profile
{
    public RespuestaProfile()
    {
        CreateMap<Respuesta, RespuestaDto>().ReverseMap();
        CreateMap<CreateRespuestaDto, Respuesta>()
            .ForMember(dest => dest.ValorRespuesta, opt => opt.MapFrom(src => src.ValorRespuesta))
            .ForMember(dest => dest.PreguntaId, opt => opt.MapFrom(src => src.PreguntaId))
            .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId))
            .ForMember(dest => dest.TrabajadorId, opt => opt.MapFrom(src => src.TrabajadorId))
            .ForMember(dest => dest.NoEvaluacion, opt => opt.MapFrom(src => src.NoEvaluacion));
        CreateMap<UpdateRespuestaDto, Respuesta>()
            .ForMember(dest => dest.ValorRespuesta, opt => opt.MapFrom(src => src.ValorRespuesta))
            .ForMember(dest => dest.PreguntaId, opt => opt.MapFrom(src => src.PreguntaId))
            .ForMember(dest => dest.UsuarioId, opt => opt.MapFrom(src => src.UsuarioId))
            .ForMember(dest => dest.TrabajadorId, opt => opt.MapFrom(src => src.TrabajadorId))
            .ForMember(dest => dest.NoEvaluacion, opt => opt.MapFrom(src => src.NoEvaluacion));
    }
}
