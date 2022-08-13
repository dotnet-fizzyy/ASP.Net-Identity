using IdentityWebApi.ApplicationLogic.Validation;

namespace IdentityWebApi.ApplicationSettings;

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
}