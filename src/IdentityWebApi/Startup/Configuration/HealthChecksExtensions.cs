using HealthChecks.UI.Client;

using IdentityWebApi.Infrastructure.Database;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityWebApi.Startup.Configuration;

/// <summary>
/// Health-Check UI configuration.
/// </summary>
internal static class HealthChecksExtensions
{
    private const string HealthCheckRoute = "/health-check";

    /// <summary>
    /// Registers Health-Check services for Infrastructure layer.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <param name="connectionString">DB connection string.</param>
    public static void RegisterHealthChecks(this IServiceCollection services, string connectionString)
    {
        services
            .AddHealthChecks()
            .AddDbContextCheck<DatabaseContext>();

        services
            .AddHealthChecksUI(options =>
            {
                options.AddHealthCheckEndpoint("HealthCheck API", HealthCheckRoute);
            })
            .AddSqlServerStorage(connectionString);
    }

    /// <summary>
    /// Adds endpoint for Health-Checks UI.
    /// </summary>
    /// <param name="endpoints"><see cref="IEndpointRouteBuilder"/>.</param>
    public static void RegisterHealthCheckEndpoint(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks(HealthCheckRoute, new HealthCheckOptions
        {
            Predicate = _ => true,
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
        });

        endpoints.MapHealthChecksUI(options => { options.UIPath = "/health-check-ui"; });
    }
}
