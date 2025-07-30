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
            .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
        CreateMap<UpdateEntrevistaDto, Entrevista>()
            .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt));
    }
}
