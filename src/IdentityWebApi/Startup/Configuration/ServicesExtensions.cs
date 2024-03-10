using IdentityWebApi.Core.Interfaces.Infrastructure;
using IdentityWebApi.Core.Interfaces.Presentation;
using IdentityWebApi.Infrastructure.Network.Services;
using IdentityWebApi.Presentation.Services;
using IdentityWebApi.Startup.ApplicationSettings;

using Microsoft.Extensions.DependencyInjection;

namespace IdentityWebApi.Startup.Configuration;

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
        services.AddSingleton<INetService, NetService>();

        services.AddScoped<IHttpContextService, HttpContextService>();
    }
}
