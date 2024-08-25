using AutoMapper;

using DY.Auth.Identity.Api.ApplicationLogic.Services.EmailTemplate.Queries.GetEmailTemplateById;
using DY.Auth.Identity.Api.Presentation.Models.DTO.EmailTemplate;

namespace DY.Auth.Identity.Api.Presentation.Mapping;

/// <summary>
/// Email template mapping profile.
/// </summary>
public class EmailTemplateProfile : Profile
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EmailTemplateProfile"/> class.
    /// </summary>
    public EmailTemplateProfile()
    {
        this.CreateMap<GetEmailTemplateByIdResult, EmailTemplateDto>()
            .ForMember(dest => dest.EmailTemplateId, opt => opt.MapFrom(src => src.EmailTemplateId))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Layout, opt => opt.MapFrom(src => src.Layout))
            .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate));
    }
}
