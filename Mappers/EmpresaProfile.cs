using AutoMapper;
using api_iso_med_pg.Models;
using api_iso_med_pg.DTOs.Empresa;

namespace api_iso_med_pg.Mappers
{
    public class EmpresaProfile : Profile
    {
        public EmpresaProfile()
        {
            CreateMap<Empresa, EmpresaDto>().ReverseMap();
            CreateMap<CreateEmpresaDto, Empresa>();
            CreateMap<UpdateEmpresaDto, Empresa>();
        }
    }
}
