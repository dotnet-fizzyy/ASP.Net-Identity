using IdentityWebApi.Core.Enums;

using NetEscapades.Configuration.Validation;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityWebApi.ApplicationSettings;

/// <summary>
/// Identity settings.
/// </summary>
public class IdentitySettings : IValidatable
{
    /// <summary>
    /// Gets or sets default role names.
    /// </summary>
    [Required]
    public ICollection<string> Roles { get; set; } = new List<string>();

    /// <summary>
    /// Gets or sets collection of <see cref="IdentitySettingsPassword"/>.
    /// </summary>
    public IdentitySettingsPassword Password { get; set; }

    /// <summary>
    /// Gets or sets collection of <see cref="DefaultUserSettings"/>.
    /// </summary>
    public ICollection<DefaultUserSettings> DefaultUsers { get; set; } = new List<DefaultUserSettings>();

    /// <summary>
    /// Gets or sets <see cref="EmailSettings"/>.
    /// </summary>
    public EmailSettings Email { get; set; }

    /// <summary>
    /// Gets or sets app authentication type ("Jwt" or "Cookies").
    /// </summary>
    public AuthType AuthType { get; set; }

    /// <summary>
    /// Gets or sets <see cref="JwtSettings"/>.
    /// </summary>
    public JwtSettings Jwt { get; set; }

    /// <summary>
    /// Gets or sets <see cref="CookiesSettings"/>.
    /// </summary>
    public CookiesSettings Cookies { get; set; }

    /// <summary>
    /// Validates settings properties.
    /// </summary>
    public void Validate()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);

        foreach (var user in this.DefaultUsers)
        {
            user.Validate();
        }

        this.Password.Validate();
        this.Cookies.Validate();
    }
}
