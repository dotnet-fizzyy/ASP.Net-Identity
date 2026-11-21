using DY.Auth.Identity.Api.ApplicationLogic.EmailTemplate.Commands.CreateEmailTemplate;
using DY.Auth.Identity.Api.ApplicationLogic.EmailTemplate.Queries.GetEmailTemplateById;
using DY.Auth.Identity.Api.Presentation.Models.DTO.EmailTemplate;

namespace DY.Auth.Identity.Api.Presentation.Mapping;

internal static class EmailTemplateMapper
{
    internal static CreateEmailTemplateCommand ToCreateEmailTemplateCommand(CreateEmailTemplateDto source) =>
        new (source.Name, source.Layout);

    internal static EmailTemplateDto ToEmailTemplateDto(CreateEmailTemplateResult source) =>
        new ()
        {
            EmailTemplateId = source.EmailTemplateId,
            Name = source.Name,
            Layout = source.Layout,
            CreationDate = source.CreationDate,
        };

    internal static EmailTemplateDto ToEmailTemplateDto(GetEmailTemplateByIdResult source) =>
        new ()
        {
            EmailTemplateId = source.EmailTemplateId,
            Name = source.Name,
            Layout = source.Layout,
            CreationDate = source.CreationDate,
        };
}
