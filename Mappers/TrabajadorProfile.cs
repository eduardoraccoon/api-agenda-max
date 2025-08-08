using AutoMapper;
using api_iso_med_pg.Models;
using api_iso_med_pg.DTOs.Trabajador;

namespace api_iso_med_pg.Mappers;

public class TrabajadorProfile : Profile
{
    public TrabajadorProfile()
    {
        CreateMap<Trabajador, TrabajadorDto>().ReverseMap();
        CreateMap<CreateTrabajadorDto, Trabajador>()
            .ForMember(dest => dest.Nombres, opt => opt.MapFrom(src => src.Nombres))
            .ForMember(dest => dest.Estatus, opt => opt.MapFrom(src => src.Estatus));
        CreateMap<UpdateTrabajadorDto, Trabajador>()
            .ForMember(dest => dest.Nombres, opt => opt.MapFrom(src => src.Nombres))
            .ForMember(dest => dest.Estatus, opt => opt.MapFrom(src => src.Estatus));
    }
}
