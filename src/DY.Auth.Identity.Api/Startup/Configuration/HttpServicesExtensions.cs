using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Startup.ApplicationSettings;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace DY.Auth.Identity.Api.Startup.Configuration;

/// <summary>
/// Internal application HTTP services configuration.
/// </summary>
internal static class HttpServicesExtensions
{
    /// <summary>
    /// Registers internal application HTTP services.
    /// </summary>
    /// <param name="services">The instance <see cref="IServiceCollection"/>.</param>
    /// <param name="appSettings">The instance of <see cref="AppSettings"/> reprsenting application settings.</param>
    public static void RegisterHttpClients(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddHttpClient(InternalApi.RegionVerification.ToString(), client =>
        {
            client.BaseAddress = new Uri(appSettings.IpStackSettings.Url);
        });
    }
}
