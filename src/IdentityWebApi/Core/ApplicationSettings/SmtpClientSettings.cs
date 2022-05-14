using IdentityWebApi.ApplicationLogic.Validation;

using NetEscapades.Configuration.Validation;

using System.ComponentModel.DataAnnotations;

namespace IdentityWebApi.Core.ApplicationSettings;

/// <summary>
/// Email client settings.
/// </summary>
public class SmtpClientSettings : IValidatable
{
    /// <summary>
    /// Gets or sets host.
    /// </summary>
    [Required]
    public string Host { get; set; }

    /// <summary>
    /// Gets or sets port.
    /// </summary>
    [DefaultValue]
    public int Port { get; set; }

    /// <summary>
    /// Gets or sets email name.
    /// </summary>
    [Required]
    public string EmailName { get; set; }

    /// <summary>
    /// Gets or sets email address.
    /// </summary>
    [Required]
    public string EmailAddress { get; set; }

    /// <summary>
    /// Gets or sets email password.
    /// </summary>
    [Required]
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether email client should use SSL.
    /// </summary>
    public bool UseSsl { get; set; }

    /// <summary>
    /// Validates settings properties.
    /// </summary>
    public void Validate()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);
    }
}
