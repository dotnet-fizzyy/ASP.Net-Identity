using IdentityWebApi.Startup.ApplicationSettings;

using Microsoft.Extensions.Configuration;

namespace IdentityWebApi.Startup.Configuration;

/// <summary>
/// Application settings extensions.
/// </summary>
public static class AppSettingsExtensions
{
    /// <summary>
    /// Reads application settings from "appsettings.json" file.
    /// </summary>
    /// <param name="configuration">
    /// Instance of <see cref="IConfiguration"/>.
    /// </param>
    /// <returns>
    /// Instance of <see cref="AppSettings"/> that contains all settings read from "appsettings.json".
    /// </returns>
    public static AppSettings ReadAppSettings(this IConfiguration configuration)
    {
        var apiSettings = configuration
            .GetSection(nameof(AppSettings.Api))
            .Get<ApiSettings>();
        var dbSettings = configuration
            .GetSection(nameof(AppSettings.DbSettings))
            .Get<DbSettings>();
        var smtpClientSettings = configuration
            .GetSection(nameof(AppSettings.SmtpClientSettings))
            .Get<SmtpClientSettings>();
        var ipStackSettings = configuration
            .GetSection(nameof(AppSettings.IpStackSettings))
            .Get<IpStackSettings>();
        var regionVerification = configuration
            .GetSection(nameof(AppSettings.RegionsVerificationSettings))
            .Get<RegionsVerificationSettings>();
        var identitySettings = configuration
            .GetSection(nameof(AppSettings.IdentitySettings))
            .Get<IdentitySettings>();

        return new AppSettings
        {
            Api = apiSettings,
            DbSettings = dbSettings,
            SmtpClientSettings = smtpClientSettings,
            IpStackSettings = ipStackSettings,
            RegionsVerificationSettings = regionVerification,
            IdentitySettings = identitySettings,
        };
    }
}