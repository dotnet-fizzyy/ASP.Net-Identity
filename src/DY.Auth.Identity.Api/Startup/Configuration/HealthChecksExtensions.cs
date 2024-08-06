using DY.Auth.Identity.Api.Infrastructure.Database;

using HealthChecks.UI.Client;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace DY.Auth.Identity.Api.Startup.Configuration;

/// <summary>
/// Health-Check UI configuration.
/// </summary>
internal static class HealthChecksExtensions
{
    private const string HealthCheckRoute = "health-check";

    /// <summary>
    /// Registers Health-Check services for Infrastructure layer.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <param name="appUrl">API url.</param>
    /// <param name="connectionString">DB connection string.</param>
    /// <param name="enableHealthCheckUi">Enable Health-Check UI or not.</param>
    public static void RegisterHealthChecks(
        this IServiceCollection services,
        string appUrl,
        string connectionString,
        bool enableHealthCheckUi)
    {
        services
            .AddHealthChecks()
            .AddDbContextCheck<DatabaseContext>();

        if (enableHealthCheckUi)
        {
            services
                .AddHealthChecksUI(options =>
                {
                    options.AddHealthCheckEndpoint("HealthCheck API", $"{appUrl}/{HealthCheckRoute}");
                })
                .AddSqlServerStorage(connectionString);
        }
    }

    /// <summary>
    /// Adds endpoint for Health-Checks UI.
    /// </summary>
    /// <param name="endpoints"><see cref="IEndpointRouteBuilder"/>.</param>
    /// <param name="enableHealthCheckUi">Enable Health-Check UI or not.</param>
    public static void RegisterHealthCheckEndpoint(this IEndpointRouteBuilder endpoints, bool enableHealthCheckUi)
    {
        endpoints.MapHealthChecks(HealthCheckRoute, new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });

        if (enableHealthCheckUi)
        {
            endpoints.MapHealthChecksUI(options => { options.UIPath = "/health-check-ui"; });
        }
    }
}
