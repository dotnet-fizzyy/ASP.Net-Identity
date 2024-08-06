using NetEscapades.Configuration.Validation;

namespace DY.Auth.Identity.Api.Startup.ApplicationSettings;

/// <summary>
/// General application settings.
/// </summary>
public class AppSettings : IValidatable
{
    /// <summary>
    /// Gets or sets <see cref="DY.Auth.Identity.Api.Startup.ApplicationSettings.ApiSettings" />.
    /// </summary>
    public ApiSettings ApiSettings { get; set; }

    /// <summary>
    /// Gets or sets <see cref="DbSettings"/>.
    /// </summary>
    public DbSettings DbSettings { get; set; }

    /// <summary>
    /// Gets or sets <see cref="SmtpClientSettings"/>.
    /// </summary>
    public SmtpClientSettings SmtpClientSettings { get; set; }

    /// <summary>
    /// Gets or sets <see cref="RegionsVerificationSettings"/>.
    /// </summary>
    public RegionsVerificationSettings RegionsVerificationSettings { get; set; }

    /// <summary>
    /// Gets or sets <see cref="IpStackSettings"/>.
    /// </summary>
    public IpStackSettings IpStackSettings { get; set; }

    /// <summary>
    /// Gets or sets <see cref="IdentitySettings"/>.
    /// </summary>
    public IdentitySettings IdentitySettings { get; set; }

    /// <summary>
    /// Gets or sets <see cref="ApplicationSettings.TelemetrySettings"/>.
    /// </summary>
    public TelemetrySettings TelemetrySettings { get; set; }

    /// <summary>
    /// Validates settings of nested parameters.
    /// </summary>
    public void Validate()
    {
        this.ApiSettings.Validate();
        this.DbSettings.Validate();
        this.SmtpClientSettings.Validate();
        this.RegionsVerificationSettings.Validate();
        this.IdentitySettings.Validate();
        this.TelemetrySettings.Validate();
    }
}
