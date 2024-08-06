using AutoMapper;

using DY.Auth.Identity.Api.ApplicationLogic.Models.Action;
using DY.Auth.Identity.Api.Core.Entities;

namespace DY.Auth.Identity.Api.ApplicationLogic.Mappers;

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
