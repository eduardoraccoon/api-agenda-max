using AutoMapper;
using api_iso_med_pg.Models;
using api_iso_med_pg.DTOs.Clients;

namespace api_iso_med_pg.Mappers
{
    public class ClienteProfile : Profile
    {
        public ClienteProfile()
        {
            CreateMap<Cliente, ClienteDto>().ReverseMap();
            CreateMap<CreateClienteDto, Cliente>();
            CreateMap<UpdateClienteDto, Cliente>();
        }
    }
}
