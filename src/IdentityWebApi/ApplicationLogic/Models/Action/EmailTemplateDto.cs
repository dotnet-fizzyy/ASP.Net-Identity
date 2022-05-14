using System;

namespace IdentityWebApi.ApplicationLogic.Models.Action;

/// <summary>
/// EmailTemplate DTO model.
/// </summary>
public class EmailTemplateDto
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
