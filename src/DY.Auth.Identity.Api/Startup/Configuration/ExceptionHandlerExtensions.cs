using DY.Auth.Identity.Api.Presentation.Middleware;

using Microsoft.Extensions.DependencyInjection;

namespace DY.Auth.Identity.Api.Startup.Configuration;

/// <summary>
/// Exception handling configuration.
/// </summary>
internal static class ExceptionHandlerExtensions
{
    /// <summary>
    /// Registers applications exception handlers.
    /// </summary>
    /// <param name="services">The instance of <see cref="IServiceCollection"/>.</param>
    public static void RegisterExceptionHandler(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
    }
}
