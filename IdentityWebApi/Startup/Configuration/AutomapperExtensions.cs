using Microsoft.Extensions.DependencyInjection;

namespace IdentityWebApi.Startup.Configuration;

public static class AutomapperExtensions
{
    public static void RegisterAutomapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Startup));
    }
}
