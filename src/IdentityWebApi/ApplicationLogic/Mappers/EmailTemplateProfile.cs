using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Entities;

namespace IdentityWebApi.ApplicationLogic.Mappers;

public class EmailTemplateProfile : Profile
{
    public EmailTemplateProfile()
    {
        CreateMap<EmailTemplateDto, EmailTemplate>()
            .ForMember(dist => dist.Id, opt => opt.MapFrom(x => x.EmailTemplateId));
        CreateMap<EmailTemplate, EmailTemplateDto>()
            .ForMember(dist => dist.EmailTemplateId, opt => opt.MapFrom(x => x.Id));
    }
}
