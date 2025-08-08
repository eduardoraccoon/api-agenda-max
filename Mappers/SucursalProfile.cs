using AutoMapper;
using api_iso_med_pg.Models;
using api_iso_med_pg.DTOs.Sucursal;

namespace api_iso_med_pg.Mappers;

public class SucursalProfile : Profile
{
    public SucursalProfile()
    {
        CreateMap<Sucursal, SucursalDto>();
        CreateMap<CreateSucursalDto, Sucursal>()
            .ForMember(dest => dest.Suc, opt => opt.MapFrom(src => src.Suc))
            .ForMember(dest => dest.Nomenclatura, opt => opt.MapFrom(src => src.Nomenclatura))
            .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Direccion))
            .ForMember(dest => dest.Ciudad, opt => opt.MapFrom(src => src.Ciudad))
            .ForMember(dest => dest.CompaniaId, opt => opt.MapFrom(src => src.CompaniaId));
        CreateMap<UpdateSucursalDto, Sucursal>()
            .ForMember(dest => dest.Suc, opt => opt.MapFrom(src => src.Suc))
            .ForMember(dest => dest.Nomenclatura, opt => opt.MapFrom(src => src.Nomenclatura))
            .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Direccion))
            .ForMember(dest => dest.Ciudad, opt => opt.MapFrom(src => src.Ciudad))
            .ForMember(dest => dest.CompaniaId, opt => opt.MapFrom(src => src.CompaniaId));
        CreateMap<Sucursal, UpdateSucursalDto>();
    }
}
