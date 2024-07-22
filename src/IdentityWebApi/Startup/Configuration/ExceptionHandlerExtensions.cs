using IdentityWebApi.Core.Exceptions;
using IdentityWebApi.Presentation.Models.Response;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

using OpenTelemetry.Trace;

using System;
using System.Diagnostics;
using System.Linq;

namespace IdentityWebApi.Startup.Configuration;

/// <summary>
/// Exception handling configuration.
/// </summary>
internal static class ExceptionHandlerExtensions
{
    /// <summary>
    /// Registers exception handler as part of middleware.
    /// </summary>
    /// <param name="app"><see cref="IApplicationBuilder"/>.</param>
    public static void RegisterExceptionHandler(this IApplicationBuilder app)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature != null)
                {
                    string errorMessage;

                    switch (contextFeature.Error)
                    {
                        case ModelValidationException modelValidationException:
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            errorMessage = modelValidationException.Errors.Aggregate((acc, message) => acc + $", {message}");

                            break;
                        case OperationCanceledException canceledException:
                            context.Response.StatusCode = StatusCodes.Status400BadRequest;
                            errorMessage = canceledException.Message;

                            break;
                        default:
                            errorMessage = contextFeature.Error.Message;

                            break;
                    }

                    AddTelemetryTags(contextFeature);

                    await context.Response.WriteAsJsonAsync(
                        new ErrorResponse(contextFeature.Error.Source, errorMessage));
                }
            });
        });
    }

    private static void AddTelemetryTags(IExceptionHandlerFeature contextFeature)
    {
        var parent = Activity.Current;

        parent?.RecordException(contextFeature.Error);
    }
}
