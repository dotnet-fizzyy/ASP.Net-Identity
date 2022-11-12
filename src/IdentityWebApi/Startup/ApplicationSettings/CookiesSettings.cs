using IdentityWebApi.ApplicationLogic.Validation;

using NetEscapades.Configuration.Validation;

using System.ComponentModel.DataAnnotations;

namespace IdentityWebApi.Startup.ApplicationSettings;

/// <summary>
/// Cookies settings.
/// </summary>
public class CookiesSettings : IValidatable
{
    /// <summary>
    /// Gets or sets a value indicating whether sliding expiration is available.
    /// </summary>
    public bool SlidingExpiration { get; set; }

    /// <summary>
    /// Gets or sets expiration minutes.
    /// </summary>
    [DefaultValue]
    public int ExpirationMinutes { get; set; }

    /// <summary>
    /// Validates settings properties.
    /// </summary>
    public void Validate()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);
    }
}
