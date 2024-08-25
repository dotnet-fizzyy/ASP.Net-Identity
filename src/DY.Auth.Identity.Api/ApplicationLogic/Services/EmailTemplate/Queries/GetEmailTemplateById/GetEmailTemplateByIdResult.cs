using System;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.EmailTemplate.Queries.GetEmailTemplateById;

/// <summary>
/// Get email template by id result model.
/// </summary>
public record GetEmailTemplateByIdResult
{
    /// <summary>
    /// Gets email template Id.
    /// </summary>
    public Guid EmailTemplateId { get; init; }

    /// <summary>
    /// Gets email template name.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Gets layout.
    /// </summary>
    public string Layout { get; init; }

    /// <summary>
    /// Gets creation date.
    /// </summary>
    public DateTime CreationDate { get; init; }
}
