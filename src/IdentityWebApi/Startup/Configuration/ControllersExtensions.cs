using IdentityWebApi.Presentation.Filters;

using Microsoft.Extensions.DependencyInjection;

using System.Text.Json.Serialization;

namespace IdentityWebApi.Startup.Configuration;

/// <summary>
/// Controllers configuration.
/// </summary>
internal static class ControllersExtensions
{
    /// <summary>
    /// Registers controllers configuration.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    public static void RegisterControllers(this IServiceCollection services)
    {
        services
            .AddControllers(options => { options.Filters.Add(typeof(RegionVerificationFilter)); })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
            });
    }
}
