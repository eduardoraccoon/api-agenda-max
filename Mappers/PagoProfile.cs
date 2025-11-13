using AutoMapper;
using api_iso_med_pg.Models;
using api_iso_med_pg.DTOs.Pago;

namespace api_iso_med_pg.Mappers
{
    public class PagoProfile : Profile
    {
        public PagoProfile()
        {
            CreateMap<Pago, PagoDto>()
                .ForMember(dest => dest.TrabajoDescripcion, opt => opt.MapFrom(src => src.Trabajo.Descripcion));
            CreateMap<CreatePagoDto, Pago>();
            CreateMap<UpdatePagoDto, Pago>();
        }
    }
}
