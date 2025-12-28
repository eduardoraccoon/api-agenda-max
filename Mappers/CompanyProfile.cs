using AutoMapper;
using api_iso_med_pg.Models;
using api_iso_med_pg.DTOs.Companies;

namespace api_iso_med_pg.Mappers
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            CreateMap<Company, CompanyDto>().ReverseMap();
            CreateMap<CreateCompanyDto, Company>();
            CreateMap<UpdateCompanyDto, Company>();
        }
    }
}
