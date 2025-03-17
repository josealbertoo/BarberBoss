using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;

namespace BarberBoss.Application.AutoMapper;
public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
    }

    private void RequestToEntity()
    {
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password, config => config.Ignore());

        CreateMap<RequestInvoiceJson, Invoice>()
            .ForMember(dest => dest.Tags, config => config.MapFrom(source => source.Tags.Distinct()));

        CreateMap<Communication.Enums.Tag, Tag>()
            .ForMember(dest => dest.Value, config => config.MapFrom(source => source));
    }

    private void EntityToResponse()
    {
        CreateMap<Invoice, ResponseInvoiceJson>()
            .ForMember(dest => dest.Tags, config => config.MapFrom(source => source.Tags.Select(tag => tag.Value)));

        CreateMap<Invoice, ResponseRegisteredInvoiceJson>();
        CreateMap<Invoice, ResponseShortInvoiceJson>();
        CreateMap<User, ResponseUserProfileJson>();
    }
}
