using AutoMapper;
using api_iso_med_pg.Models;
using api_iso_med_pg.DTOs.Works;

namespace api_iso_med_pg.Mappers
{
    public class WorkProfile : Profile
    {
        public WorkProfile()
        {
            CreateMap<Work, WorkDto>()
                .ForMember(dest => dest.ClientName, opt => opt.MapFrom(src => src.Client.Name));
            CreateMap<CreateWorkDto, Work>();
            CreateMap<UpdateWorkDto, Work>();
        }
    }
}
