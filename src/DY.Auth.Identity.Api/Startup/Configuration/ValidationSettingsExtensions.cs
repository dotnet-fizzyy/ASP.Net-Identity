using DY.Auth.Identity.Api.ApplicationLogic.Pipelines;

using FluentValidation;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using System.Reflection;

namespace DY.Auth.Identity.Api.Startup.Configuration;

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
