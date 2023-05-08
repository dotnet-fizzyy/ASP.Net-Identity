using System.Reflection;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

namespace IdentityWebApi.Startup.Configuration;

/// <summary>
/// Mediatr configuration.
/// </summary>
public static class MediatrExtensions
{
    /// <summary>
    /// Registers Mediatr service.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    public static void RegisterMediatr(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
    }
}
