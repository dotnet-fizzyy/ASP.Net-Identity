using DY.Auth.Identity.Api.Core.Utilities;

using NetEscapades.Configuration.Validation;

using System.ComponentModel.DataAnnotations;

namespace DY.Auth.Identity.Api.Startup.ApplicationSettings;

/// <summary>
/// Gets or sets API settings.
/// </summary>
public class ApiSettings : IValidatable
{
    /// <summary>
    /// Gets or sets API URL.
    /// </summary>
    [DefaultValue]
    public string Url { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether Health-Check UI should be enabled or not.
    /// </summary>
    public bool EnableHealthCheckUi { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether Swagger should be enabled or not.
    /// </summary>
    public bool EnableSwagger { get; set; }

    /// <inheritdoc />
    public void Validate()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);
    }
}
