using AutoMapper;
using api_iso_med_pg.Models;
using api_iso_med_pg.DTOs.Compania;

namespace api_iso_med_pg.Mappers;

public class CompaniaProfile : Profile
{
    public CompaniaProfile()
    {
        CreateMap<Compania, CompaniaDto>();
        CreateMap<CreateCompaniaDto, Compania>();
        CreateMap<UpdateCompaniaDto, Compania>().ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Compania, UpdateCompaniaDto>();
    }
}
