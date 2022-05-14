using IdentityWebApi.ApplicationLogic.Validation;

using NetEscapades.Configuration.Validation;

using System.ComponentModel.DataAnnotations;

namespace IdentityWebApi.Core.ApplicationSettings;

/// <summary>
/// Identity Server settings password.
/// </summary>
public class IdentitySettingsPassword : IValidatable
{
    /// <summary>
    /// Gets or sets a value indicating whether password requires digit symbols.
    /// </summary>
    public bool RequireDigit { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether password requires lowercase symbols.
    /// </summary>
    public bool RequireLowercase { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether password requires uppercase symbols.
    /// </summary>
    public bool RequireUppercase { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether password requires non alphanumeric symbols.
    /// </summary>
    public bool RequireNonAlphanumeric { get; set; }

    /// <summary>
    /// Gets or sets required password length.
    /// </summary>
    [DefaultValue]
    public int RequiredLength { get; set; }

    /// <summary>
    /// Gets or sets required password amount of unique chars.
    /// </summary>
    [DefaultValue]
    public int RequiredUniqueChars { get; set; }

    /// <summary>
    /// Validates settings properties.
    /// </summary>
    public void Validate()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);
    }
}
