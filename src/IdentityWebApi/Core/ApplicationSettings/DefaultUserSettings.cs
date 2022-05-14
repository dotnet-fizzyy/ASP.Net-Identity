using System.ComponentModel.DataAnnotations;

namespace IdentityWebApi.Core.ApplicationSettings;

/// <summary>
/// Default user settings.
/// </summary>
public class DefaultUserSettings
{
    /// <summary>
    /// Gets or sets name.
    /// </summary>
    [Required]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets password.
    /// </summary>
    [Required]
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets role.
    /// </summary>
    [Required]
    public string Role { get; set; }

    /// <summary>
    /// Gets or sets email.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Validates settings properties.
    /// </summary>
    public void Validate()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);
    }
}
