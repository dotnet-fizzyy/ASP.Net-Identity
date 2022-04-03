using AutoMapper;

using IdentityWebApi.Core.Entities;
using IdentityWebApi.Presentation.Models.DTO;

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
