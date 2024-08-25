using System;

namespace DY.Auth.Identity.Api.Presentation.Models.DTO.EmailTemplate;

/// <summary>
/// Email template DTO model.
/// </summary>
public record EmailTemplateDto
{
    /// <summary>
    /// Gets or sets email template Id.
    /// </summary>
    public Guid EmailTemplateId { get; set; }

    /// <summary>
    /// Gets or sets email template name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets layout.
    /// </summary>
    public string Layout { get; set; }

    /// <summary>
    /// Gets or sets creation date.
    /// </summary>
    public DateTime CreationDate { get; set; }
}
