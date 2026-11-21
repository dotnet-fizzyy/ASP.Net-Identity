using DY.Auth.Identity.Api.ApplicationLogic.EmailTemplate.Commands.CreateEmailTemplate;
using DY.Auth.Identity.Api.ApplicationLogic.EmailTemplate.Queries.GetEmailTemplateById;

using EmailTemplateEntity = DY.Auth.Identity.Api.Core.Entities.EmailTemplate;

namespace DY.Auth.Identity.Api.ApplicationLogic.Shared.Mapping;

/// <summary>
/// Email template feature mapper.
/// </summary>
internal static class EmailTemplateMapper
{
    /// <summary>
    /// Maps <see cref="CreateEmailTemplateCommand"/> to <see cref="EmailTemplateEntity"/>.
    /// </summary>
    /// <param name="source">The create-email-template command to map.</param>
    /// <returns>The mapped entity, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static EmailTemplateEntity ToEmailTemplateEntity(CreateEmailTemplateCommand source)
    {
        if (source is null)
        {
            return null;
        }

        return new EmailTemplateEntity
        {
            Name = source.Name,
            Layout = source.Layout,
        };
    }

    /// <summary>
    /// Maps <see cref="EmailTemplateEntity"/> to <see cref="CreateEmailTemplateResult"/>.
    /// </summary>
    /// <param name="source">The email template entity to map.</param>
    /// <returns>The mapped result, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static CreateEmailTemplateResult ToCreateEmailTemplateResult(EmailTemplateEntity source)
    {
        if (source is null)
        {
            return null;
        }

        return new CreateEmailTemplateResult
        {
            EmailTemplateId = source.Id,
            Name = source.Name,
            Layout = source.Layout,
            CreationDate = source.CreationDate,
        };
    }

    /// <summary>
    /// Maps <see cref="EmailTemplateEntity"/> to <see cref="GetEmailTemplateByIdResult"/>.
    /// </summary>
    /// <param name="source">The email template entity to map.</param>
    /// <returns>The mapped result, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static GetEmailTemplateByIdResult ToGetEmailTemplateByIdResult(EmailTemplateEntity source)
    {
        if (source is null)
        {
            return null;
        }

        return new GetEmailTemplateByIdResult
        {
            EmailTemplateId = source.Id,
            Name = source.Name,
            Layout = source.Layout,
            CreationDate = source.CreationDate,
        };
    }
}
