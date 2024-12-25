using DY.Auth.Identity.Api.Core.Exceptions;
using DY.Auth.Identity.Api.Presentation.Models.Response;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.Presentation.Middleware;

/// <summary>
/// Application global exception handler.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private const string JsonContentType = "application/json";

    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        string errorMessage;

        switch (exception)
        {
            case ModelValidationException modelValidationException:
                HandleModelValidationException(httpContext, modelValidationException, out errorMessage);
                break;
            case OperationCanceledException canceledException:
                HandleOperationCancelledException(httpContext, canceledException, out errorMessage);
                break;
            default:
                HandleUnknownException(httpContext, exception, out errorMessage);
                break;
        }

        AddTelemetryTags(exception);

        await httpContext.Response.WriteAsJsonAsync(
            new ErrorResponse(exception.Source, errorMessage),
            cancellationToken);

        return true;
    }

    private static void HandleModelValidationException(
        HttpContext httpContext,
        ModelValidationException modelValidationException,
        out string errorMessage)
    {
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        httpContext.Response.ContentType = JsonContentType;

        errorMessage = modelValidationException.Errors.Aggregate((acc, message) => acc + $", {message}");
    }

    private static void HandleOperationCancelledException(
        HttpContext httpContext,
        OperationCanceledException canceledException,
        out string errorMessage)
    {
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        httpContext.Response.ContentType = JsonContentType;

        errorMessage = canceledException.Message;
    }

    private static void HandleUnknownException(
        HttpContext httpContext,
        Exception exception,
        out string errorMessage)
    {
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = JsonContentType;

        errorMessage = exception.Message;
    }

    private static void AddTelemetryTags(Exception exception)
    {
        var parent = Activity.Current;

        parent?.AddException(exception);
    }
}
