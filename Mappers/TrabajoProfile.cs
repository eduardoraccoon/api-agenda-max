using AutoMapper;
using api_iso_med_pg.Models;
using api_iso_med_pg.DTOs.Trabajos;

namespace api_iso_med_pg.Mappers
{
    public class TrabajoProfile : Profile
    {
        public TrabajoProfile()
        {
            CreateMap<Trabajo, TrabajoDto>()
                .ForMember(dest => dest.ClienteNombre, opt => opt.MapFrom(src => src.Cliente.Nombre));
            CreateMap<CreateTrabajoDto, Trabajo>();
            CreateMap<UpdateTrabajoDto, Trabajo>();
        }
    }
}
