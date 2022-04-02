using HealthChecks.UI.Client;

using IdentityWebApi.DAL;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityWebApi.Startup.Configuration
{
    public static class HealthChecksExtensions
    {
        private const string HealthCheckRoute = "/health-check";
        
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

        public static void RegisterHealthCheckEndpoint(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecks(HealthCheckRoute, new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            
            endpoints.MapHealthChecksUI(options =>
            {
                options.UIPath = "/health-check-ui";
            });
        }
    }
}