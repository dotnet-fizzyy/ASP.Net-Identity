using DY.Auth.Identity.Api.Core.Interfaces.Infrastructure;
using DY.Auth.Identity.Api.Core.Interfaces.Presentation;
using DY.Auth.Identity.Api.Infrastructure.Network.Services;
using DY.Auth.Identity.Api.Infrastructure.Observability.Tracing;
using DY.Auth.Identity.Api.Presentation.Services;
using DY.Auth.Identity.Api.Startup.ApplicationSettings;

using Microsoft.Extensions.DependencyInjection;

namespace DY.Auth.Identity.Api.Startup.Configuration;

/// <summary>
/// Services DI configuration.
/// </summary>
internal static class ServicesExtensions
{
    /// <summary>
    /// Configures application services for DI.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <param name="appSettings"><see cref="AppSettings"/>.</param>
    public static void RegisterServices(this IServiceCollection services, AppSettings appSettings)
    {
        // AppSettings
        services.AddSingleton(appSettings);

        // Services
        services.AddTransient<IEmailService, EmailService>();
        services.AddSingleton<IRegionVerificationService, RegionVerificationService>();
        services.AddSingleton<IActivityTracing, OpenTelemetryTracing>();

        services.AddScoped<IHttpContextService, HttpContextService>();
    }
}
