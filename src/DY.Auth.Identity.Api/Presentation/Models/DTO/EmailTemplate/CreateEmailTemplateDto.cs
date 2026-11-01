namespace DY.Auth.Identity.Api.Presentation.Models.DTO.EmailTemplate;

/// <summary>
/// Create email template DTO.
/// </summary>
public class CreateEmailTemplateDto
{
    /// <summary>
    /// Gets or sets email template name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets email template layout.
    /// </summary>
    public string Layout { get; set; }
}
