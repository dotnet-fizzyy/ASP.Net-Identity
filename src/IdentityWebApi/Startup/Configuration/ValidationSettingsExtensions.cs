using FluentValidation;

using IdentityWebApi.ApplicationLogic.Pipelines;
using IdentityWebApi.Core.ApplicationSettings;

using MediatR;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

namespace IdentityWebApi.Startup.Configuration;

/// <summary>
/// Validation of appsettings.json parameters.
/// </summary>
internal static class ValidationSettingsExtensions
{
    /// <summary>
    /// Validates <see cref="AppSettings"/> parameters read from appsettings.json.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <param name="configuration"><see cref="IConfiguration"/>.</param>
    public static void ValidateSettingParameters(this IServiceCollection services, IConfiguration configuration)
    {
        services.UseConfigurationValidation();
        services.ConfigureValidatableSetting<AppSettings>(configuration);
    }

    /// <summary>
    /// Registers validation behaviour pipeline.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    public static void RegisterValidationPipeline(this IServiceCollection services)
    {
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
