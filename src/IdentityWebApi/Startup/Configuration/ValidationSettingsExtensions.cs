using IdentityWebApi.Core.ApplicationSettings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityWebApi.Startup.Configuration;

/// <summary>
/// Validation of appsettings.json parameters.
/// </summary>
internal static class ValidationSettingsExtensions
{
    /// <summary>
    /// Validates <see cref="AppSettings"/> parameters read from appsettings.json.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <param name="configuration"><see cref="IConfiguration"/>.</param>
    public static void ValidateSettingParameters(this IServiceCollection services, IConfiguration configuration)
    {
        services.UseConfigurationValidation();
        services.ConfigureValidatableSetting<AppSettings>(configuration);
    }
}
