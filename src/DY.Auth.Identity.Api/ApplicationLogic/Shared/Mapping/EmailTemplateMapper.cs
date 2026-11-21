using DY.Auth.Identity.Api.ApplicationLogic.EmailTemplate.Commands.CreateEmailTemplate;
using DY.Auth.Identity.Api.ApplicationLogic.EmailTemplate.Queries.GetEmailTemplateById;

using EmailTemplateEntity = DY.Auth.Identity.Api.Core.Entities.EmailTemplate;

namespace DY.Auth.Identity.Api.ApplicationLogic.Shared.Mapping;

/// <summary>
/// Email template feature mapper.
/// </summary>
internal static class EmailTemplateMapper
{
    internal static EmailTemplateEntity ToEmailTemplateEntity(CreateEmailTemplateCommand source) =>
        new ()
        {
            Name = source.Name,
            Layout = source.Layout,
        };

    internal static CreateEmailTemplateResult ToCreateEmailTemplateResult(EmailTemplateEntity source) =>
        new ()
        {
            EmailTemplateId = source.Id,
            Name = source.Name,
            Layout = source.Layout,
            CreationDate = source.CreationDate,
        };

    internal static GetEmailTemplateByIdResult ToGetEmailTemplateByIdResult(EmailTemplateEntity source) =>
        new ()
        {
            EmailTemplateId = source.Id,
            Name = source.Name,
            Layout = source.Layout,
            CreationDate = source.CreationDate,
        };
}
