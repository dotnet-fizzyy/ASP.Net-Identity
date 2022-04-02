using AutoMapper;

using IdentityWebApi.DAL.Entities;
using IdentityWebApi.PL.Models.DTO;

namespace IdentityWebApi.BL.Mappers;

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
