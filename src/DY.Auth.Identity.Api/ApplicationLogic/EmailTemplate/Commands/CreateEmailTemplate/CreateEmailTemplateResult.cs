using System;

namespace DY.Auth.Identity.Api.ApplicationLogic.EmailTemplate.Commands.CreateEmailTemplate;

/// <summary>
/// Create email template result model.
/// </summary>
public record CreateEmailTemplateResult
{
    /// <summary>
    /// Gets email template ID.
    /// </summary>
    public Guid EmailTemplateId { get; init; }

    /// <summary>
    /// Gets email template name.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Gets email template layout.
    /// </summary>
    public string Layout { get; init; }

    /// <summary>
    /// Gets creation date.
    /// </summary>
    public DateTime CreationDate { get; init; }
}
