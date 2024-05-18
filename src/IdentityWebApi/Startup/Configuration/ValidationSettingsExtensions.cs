using FluentValidation;

using IdentityWebApi.ApplicationLogic.Pipelines;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

namespace IdentityWebApi.Startup.Configuration;

/// <summary>
/// Validation settings registrations.
/// </summary>
internal static class ValidationSettingsExtensions
{
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
