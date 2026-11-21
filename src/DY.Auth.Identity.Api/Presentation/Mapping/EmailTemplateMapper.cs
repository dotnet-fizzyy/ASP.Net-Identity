using DY.Auth.Identity.Api.ApplicationLogic.EmailTemplate.Commands.CreateEmailTemplate;
using DY.Auth.Identity.Api.ApplicationLogic.EmailTemplate.Queries.GetEmailTemplateById;
using DY.Auth.Identity.Api.Presentation.Models.DTO.EmailTemplate;

namespace DY.Auth.Identity.Api.Presentation.Mapping;

/// <summary>
/// Maps email-template presentation models, application commands, and results.
/// </summary>
internal static class EmailTemplateMapper
{
    /// <summary>
    /// Maps <see cref="CreateEmailTemplateDto"/> to <see cref="CreateEmailTemplateCommand"/>.
    /// </summary>
    /// <param name="source">The create-email-template DTO to map.</param>
    /// <returns>The mapped command, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static CreateEmailTemplateCommand ToCreateEmailTemplateCommand(CreateEmailTemplateDto source)
    {
        if (source is null)
        {
            return null;
        }

        return new CreateEmailTemplateCommand(source.Name, source.Layout);
    }

    /// <summary>
    /// Maps <see cref="CreateEmailTemplateResult"/> to <see cref="EmailTemplateDto"/>.
    /// </summary>
    /// <param name="source">The create-email-template result to map.</param>
    /// <returns>The mapped DTO, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static EmailTemplateDto ToEmailTemplateDto(CreateEmailTemplateResult source)
    {
        if (source is null)
        {
            return null;
        }

        return new EmailTemplateDto
        {
            EmailTemplateId = source.EmailTemplateId,
            Name = source.Name,
            Layout = source.Layout,
            CreationDate = source.CreationDate,
        };
    }

    /// <summary>
    /// Maps <see cref="GetEmailTemplateByIdResult"/> to <see cref="EmailTemplateDto"/>.
    /// </summary>
    /// <param name="source">The email-template-by-id result to map.</param>
    /// <returns>The mapped DTO, or <see langword="null"/> when the source is <see langword="null"/>.</returns>
    internal static EmailTemplateDto ToEmailTemplateDto(GetEmailTemplateByIdResult source)
    {
        if (source is null)
        {
            return null;
        }

        return new EmailTemplateDto
        {
            EmailTemplateId = source.EmailTemplateId,
            Name = source.Name,
            Layout = source.Layout,
            CreationDate = source.CreationDate,
        };
    }
}
