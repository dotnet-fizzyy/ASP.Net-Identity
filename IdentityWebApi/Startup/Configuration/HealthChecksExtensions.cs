using IdentityWebApi.DAL;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityWebApi.Startup.Configuration
{
    public static class HealthChecksExtensions
    {
        public static void RegisterHealthChecks(this IServiceCollection services, string connectionString)
        {
            services
                .AddHealthChecks()
                .AddDbContextCheck<DatabaseContext>();
            
            services
                .AddHealthChecksUI(options =>
                {
                    options.AddHealthCheckEndpoint("HealthCheck API", "/health-check");
                })
                .AddSqlServerStorage(connectionString);
        }
    }
}