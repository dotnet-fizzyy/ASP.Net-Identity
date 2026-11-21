using AutoMapper;

using DY.Auth.Identity.Api.ApplicationLogic.EmailTemplate.Commands.CreateEmailTemplate;
using DY.Auth.Identity.Api.ApplicationLogic.EmailTemplate.Queries.GetEmailTemplateById;

using EmailTemplateEntity = DY.Auth.Identity.Api.Core.Entities.EmailTemplate;

namespace DY.Auth.Identity.Api.ApplicationLogic.Shared.Mapping;

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
        this.CreateMap<CreateEmailTemplateCommand, EmailTemplateEntity>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Layout, opt => opt.MapFrom(src => src.Layout));

        this.CreateMap<EmailTemplateEntity, CreateEmailTemplateResult>()
            .ForMember(dest => dest.EmailTemplateId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Layout, opt => opt.MapFrom(src => src.Layout))
            .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate));

        this.CreateMap<EmailTemplateEntity, GetEmailTemplateByIdResult>()
            .ForMember(dest => dest.EmailTemplateId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Layout, opt => opt.MapFrom(src => src.Layout))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate));
    }
}
