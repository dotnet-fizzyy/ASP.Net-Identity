using AutoMapper;

using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Entities;

namespace IdentityWebApi.ApplicationLogic.Mappers;

/// <summary>
/// Configuration of EmailTemplate models and entities mapping.
/// </summary>
public class EmailTemplateProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmailTemplateProfile"/> class with mapping of models.
    /// </summary>
    public EmailTemplateProfile()
    {
        this.CreateMap<EmailTemplateDto, EmailTemplate>()
            .ForMember(dist => dist.Id, opt => opt.MapFrom(x => x.EmailTemplateId));
        this.CreateMap<EmailTemplate, EmailTemplateDto>()
            .ForMember(dist => dist.EmailTemplateId, opt => opt.MapFrom(x => x.Id));
    }
}
