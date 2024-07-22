using IdentityWebApi.Core.Utilities;

using NetEscapades.Configuration.Validation;

using System.ComponentModel.DataAnnotations;

namespace IdentityWebApi.Startup.ApplicationSettings;

/// <summary>
/// Telemetry configuration settings.
/// </summary>
public class TelemetrySettings : IValidatable
{
    /// <summary>
    /// Gets or sets application name.
    /// </summary>
    [DefaultValue]
    public string AppName { get; set; }

    /// <summary>
    /// Gets or sets application version.
    /// </summary>
    [DefaultValue]
    public string Version { get; set; }

    /// <summary>
    /// Gets or sets application namespace.
    /// </summary>
    [DefaultValue]
    public string Namespace { get; set; }

    /// <summary>
    /// Gets or sets OTLP explorer endpoint URL.
    /// </summary>
    [DefaultValue]
    public string OtlpExplorerEndpoint { get; set; }

    /// <inheritdoc />
    public void Validate()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);
    }
}
