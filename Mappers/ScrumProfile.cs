using AutoMapper;
using api_iso_med_pg.Models;
using api_iso_med_pg.DTOs;

namespace api_iso_med_pg.Mappers
{
    public class ScrumProfile : Profile
    {
        public ScrumProfile()
        {
            CreateMap<Scrum, ScrumDto>().ReverseMap();
            CreateMap<CreateScrumDto, Scrum>()
                .ForMember(dest => dest.CreadoId, opt => opt.MapFrom(src => src.CreadoId))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.FechaCreacion));
            CreateMap<UpdateScrumDto, Scrum>()
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Start, opt => opt.MapFrom(src => src.Start))
                .ForMember(dest => dest.End, opt => opt.MapFrom(src => src.End))
                .ForMember(dest => dest.ActualizadoId, opt => opt.MapFrom(src => src.ActualizadoId))
                .ForMember(dest => dest.FechaActualizacion, opt => opt.MapFrom(src => src.FechaActualizacion));
        }
    }
}
