using IdentityWebApi.Startup.ApplicationSettings;
using IdentityWebApi.Startup.Configuration;

using Microsoft.Extensions.DependencyInjection;

namespace IdentityWebApi.Startup;

/// <summary>
/// Application services configuration layer (<c>ConfigureServices</c> in past).
/// </summary>
public static class ServicesConfigurationExtensions
{
    /// <summary>
    /// Configures services for application usage.
    /// </summary>
    /// <param name="services">
    /// Instance of <see cref="IServiceCollection"/>.
    /// </param>
    /// <param name="appSettings">
    /// Application settings from "appsettings.json" file.
    /// </param>
    public static void Configure(this IServiceCollection services, AppSettings appSettings)
    {
        services.RegisterServices(appSettings);

        // Identity server setup should go before Auth setup
        services.RegisterIdentityServer(appSettings.IdentitySettings, appSettings.DbSettings);
        services.RegisterAuthSettings(appSettings.IdentitySettings);

        services.RegisterMediatr();
        services.RegisterValidationPipeline();

        services.RegisterAutomapper();

        services.RegisterHealthChecks(
            appSettings.Api.Url,
            appSettings.DbSettings.ConnectionString,
            appSettings.Api.EnableHealthCheckUi);

        services.AddHttpContextAccessor();

        services.RegisterOpenTelemetry();

        services.AddRouting(opts =>
        {
            opts.LowercaseUrls = true;
        });

        services.RegisterControllers();

        services.RegisterSwagger(appSettings.IdentitySettings);
    }
}
