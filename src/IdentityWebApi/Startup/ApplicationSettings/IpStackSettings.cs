using System.ComponentModel.DataAnnotations;

using IdentityWebApi.Core.Utilities;

using NetEscapades.Configuration.Validation;

namespace IdentityWebApi.Startup.ApplicationSettings;

/// <summary>
/// IpStack settings.
/// </summary>
public class IpStackSettings : IValidatable
{
    /// <summary>
    /// Gets or sets IpStack access key.
    /// </summary>
    [DefaultValue]
    public string AccessKey { get; set; }

    /// <summary>
    /// Validates settings properties.
    /// </summary>
    public void Validate()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);
    }
}