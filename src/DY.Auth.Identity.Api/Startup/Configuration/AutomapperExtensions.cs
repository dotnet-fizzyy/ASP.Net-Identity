using Microsoft.Extensions.DependencyInjection;

namespace DY.Auth.Identity.Api.Startup.Configuration;

/// <summary>
/// Automapper configuration.
/// </summary>
internal static class AutomapperExtensions
{
    /// <summary>
    /// Registers Automapper client in application.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    public static void RegisterAutomapper(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(Program));
    }
}
