using IdentityWebApi.Core.Utilities;

namespace IdentityWebApi.Startup.ApplicationSettings;

/// <summary>
/// Gets or sets API settings.
/// </summary>
public class ApiSettings
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
}
