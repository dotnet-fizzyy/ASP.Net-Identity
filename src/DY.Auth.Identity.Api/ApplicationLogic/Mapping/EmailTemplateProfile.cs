using AutoMapper;

using DY.Auth.Identity.Api.ApplicationLogic.Services.EmailTemplate.Queries.GetEmailTemplateById;
using DY.Auth.Identity.Api.Core.Entities;

namespace DY.Auth.Identity.Api.ApplicationLogic.Mapping;

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
        this.CreateMap<EmailTemplate, GetEmailTemplateByIdResult>()
            .ForMember(dest => dest.EmailTemplateId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Layout, opt => opt.MapFrom(src => src.Layout))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate));
    }
}
