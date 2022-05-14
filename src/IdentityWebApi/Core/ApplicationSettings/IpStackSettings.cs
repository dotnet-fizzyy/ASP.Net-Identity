using NetEscapades.Configuration.Validation;

using System.ComponentModel.DataAnnotations;

namespace IdentityWebApi.Core.ApplicationSettings;

/// <summary>
/// IpStack settings.
/// </summary>
public class IpStackSettings : IValidatable
{
    /// <summary>
    /// Gets or sets IpStack access key.
    /// </summary>
    [Required]
    public string AccessKey { get; set; }

    /// <summary>
    /// Validates settings properties.
    /// </summary>
    public void Validate()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);
    }
}