using DY.Auth.Identity.Api.Core.Exceptions;
using DY.Auth.Identity.Api.Presentation.Models.Response;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.Presentation.Middleware;

/// <summary>
/// Application global exception handler.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private const string JsonContentType = "application/json";

    private readonly ILogger<GlobalExceptionHandler> logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalExceptionHandler"/> class.
    /// </summary>
    /// <param name="logger">The instance of <see cref="ILogger{T}"/>.</param>
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var exceptionDetails = this.ProcessErrorResponse(httpContext, exception);

        AddTelemetryTags(exception);

        await HandleErrorResponseAsync(httpContext, exceptionDetails, cancellationToken);

        return true;
    }

    private ErrorResponse ProcessErrorResponse(
        HttpContext httpContext,
        Exception exception)
    {
        return exception switch
        {
            ModelValidationException modelValidationException =>
                this.HandleModelValidationException(httpContext, modelValidationException),
            OperationCanceledException canceledException =>
                HandleOperationCancelledException(canceledException),
            _ =>
                this.HandleUnknownException(httpContext, exception),
        };
    }

    private ErrorResponse HandleModelValidationException(
        HttpContext httpContext,
        ModelValidationException modelValidationException)
    {
        var exceptionDetails = new ErrorResponse
        {
            ErrorCode = "VALIDATION_ERROR",
            Message = modelValidationException.Errors.Aggregate((acc, message) => acc + $", {message}"),
            StatusCode = StatusCodes.Status400BadRequest,
            Source = modelValidationException.Source,
        };

        this.LogError(
            exceptionDetails.ErrorId,
            exceptionDetails.ErrorCode,
            exceptionDetails.Timestamp,
            exceptionDetails.Message,
            httpContext.Request.Path,
            modelValidationException.Source,
            modelValidationException.StackTrace);

        return exceptionDetails;
    }

    private static ErrorResponse HandleOperationCancelledException(OperationCanceledException canceledException)
    {
        return new ErrorResponse
        {
            ErrorCode = "OPERATION_CANCELLED",
            Message = GetErrorMessages(canceledException),
            StatusCode = StatusCodes.Status499ClientClosedRequest,
            Source = canceledException.Source,
        };
    }

    private ErrorResponse HandleUnknownException(
        HttpContext httpContext,
        Exception exception)
    {
        var exceptionDetails = new ErrorResponse
        {
            ErrorCode = "INTERNAL_SERVER_ERROR",
            Message = GetErrorMessages(exception),
            StatusCode = StatusCodes.Status500InternalServerError,
            Source = exception.Source,
        };

        this.LogError(
            exceptionDetails.ErrorId,
            exceptionDetails.ErrorCode,
            exceptionDetails.Timestamp,
            exceptionDetails.Message,
            httpContext.Request.Path,
            exception.Source,
            exception.StackTrace);

        return exceptionDetails;
    }

    private static void AddTelemetryTags(Exception exception)
    {
        var parent = Activity.Current;

        parent?.AddException(exception);
    }

    private static string GetErrorMessages(Exception exception)
    {
        var messageBuilder = new StringBuilder(exception.Message);

        var currentException = exception;

        while (currentException?.InnerException != null)
        {
            messageBuilder.Append(';' + currentException.InnerException.Message);

            currentException = exception.InnerException;
        }

        return messageBuilder.ToString();
    }

    private static Task HandleErrorResponseAsync(
        HttpContext httpContext,
        ErrorResponse errorResponse,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = errorResponse.StatusCode;
        httpContext.Response.ContentType = JsonContentType;

        return httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);
    }

    private void LogError(
        Guid errorId,
        string errorCode,
        long timestamp,
        string message,
        string requestPath,
        string source,
        string stacktrace)
    {
        var errorMessage =
            $"""
             An error occurred in app and was handled successfully.
             Error ID: {errorId};
             Error code: {errorCode};
             Timestamp: {timestamp};
             Request path: {requestPath};
             Message: {message};
             Source: {source};
             Stacktrace: {stacktrace};
             """;

        this.logger.LogError(errorMessage);
    }
}
