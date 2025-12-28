using AutoMapper;
using api_iso_med_pg.Models;
using api_iso_med_pg.DTOs.Clients;

namespace api_iso_med_pg.Mappers
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ClientDto>().ReverseMap();
            CreateMap<CreateClientDto, Client>();
            CreateMap<UpdateClientDto, Client>();
        }
    }
}
