using IdentityWebApi.Presentation.Filters;

using Microsoft.Extensions.DependencyInjection;

using System.Text.Json.Serialization;

namespace IdentityWebApi.Startup.Configuration;

public static class ControllersExtensions
{
    public static void RegisterControllers(this IServiceCollection services)
    {
        services
            .AddControllers(options =>
            {
                options.Filters.Add(typeof(RegionVerificationFilter));
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
            });
    }
}