using IdentityWebApi.ApplicationLogic.Validation;

using System.ComponentModel.DataAnnotations;

namespace IdentityWebApi.ApplicationSettings;

/// <summary>
/// Default user settings.
/// </summary>
public class DefaultUserSettings
{
    /// <summary>
    /// Gets or sets name.
    /// </summary>
    [DefaultValue]
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets password.
    /// </summary>
    [DefaultValue]
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets role.
    /// </summary>
    [DefaultValue]
    public string Role { get; set; }

    /// <summary>
    /// Gets or sets email.
    /// </summary>
    [DefaultValue]
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
