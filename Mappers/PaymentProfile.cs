using AutoMapper;
using api_iso_med_pg.Models;
using api_iso_med_pg.DTOs.Payment;

namespace api_iso_med_pg.Mappers
{
    public class PaymentProfile : Profile
    {
        public PaymentProfile()
        {
            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.WorkDescription, opt => opt.MapFrom(src => src.Work.Description));
            CreateMap<CreatePaymentDto, Payment>();
            CreateMap<UpdatePaymentDto, Payment>();
        }
    }
}
