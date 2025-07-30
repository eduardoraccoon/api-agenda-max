using AutoMapper;
using api_iso_med_pg.DTOs.Equipamiento;
using api_iso_med_pg.Models;

namespace api_iso_med_pg.Mappers;

public class EquipamientoProfile : Profile
{
    public EquipamientoProfile()
    {
        CreateMap<Equipamiento, EquipamientoDto>().ReverseMap();
        CreateMap<CreateEquipamientoDto, Equipamiento>();
        CreateMap<UpdateEquipamientoDto, Equipamiento>();
    }
}
