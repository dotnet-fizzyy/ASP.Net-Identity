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
        this.logger = logger;
    }

    /// <inheritdoc />
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var exceptionDetails = ConstructExceptionDetails(httpContext, exception);

        AddTelemetryTags(exception);

        this.LogError(exceptionDetails);

        await HandleErrorResponseAsync(httpContext, exceptionDetails, cancellationToken);

        return true;
    }

    private static ExceptionDetails ConstructExceptionDetails(
        HttpContext httpContext,
        Exception exception)
    {
        return exception switch
        {
            ModelValidationException modelValidationException =>
                HandleModelValidationException(httpContext, modelValidationException),
            OperationCanceledException canceledException =>
                HandleOperationCancelledException(httpContext, canceledException),
            _ =>
                HandleUnknownException(httpContext, exception),
        };
    }

    private static ExceptionDetails HandleModelValidationException(
        HttpContext httpContext,
        ModelValidationException modelValidationException)
    {
        var exceptionDetails = CreateGeneralExceptionDetails(httpContext, modelValidationException);

        exceptionDetails.ErrorCode = "VALIDATION_ERROR";
        exceptionDetails.ResponseStatusCode = StatusCodes.Status400BadRequest;
        exceptionDetails.Message = modelValidationException.Errors.Aggregate((acc, message) => acc + $", {message}");

        return exceptionDetails;
    }

    private static ExceptionDetails HandleOperationCancelledException(
        HttpContext httpContext,
        OperationCanceledException canceledException)
    {
        var exceptionDetails = CreateGeneralExceptionDetails(httpContext, canceledException);

        exceptionDetails.ErrorCode = "OPERATION_CANCELLED";
        exceptionDetails.ResponseStatusCode = StatusCodes.Status499ClientClosedRequest;
        exceptionDetails.Message = GetErrorMessages(canceledException);

        return exceptionDetails;
    }

    private static ExceptionDetails HandleUnknownException(
        HttpContext httpContext,
        Exception exception)
    {
        var exceptionDetails = CreateGeneralExceptionDetails(httpContext, exception);

        exceptionDetails.ErrorCode = "INTERNAL_SERVER_ERROR";
        exceptionDetails.ResponseStatusCode = StatusCodes.Status500InternalServerError;
        exceptionDetails.Message = GetErrorMessages(exception);

        return exceptionDetails;
    }

    private static void AddTelemetryTags(Exception exception)
    {
        var parent = Activity.Current;

        parent?.AddException(exception);
    }

    private static Guid GenerateErrorId()
    {
        return Guid.CreateVersion7();
    }

    private static long GetTimestamp()
    {
        return ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
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
        ExceptionDetails exceptionDetails,
        CancellationToken cancellationToken)
    {
        httpContext.Response.StatusCode = exceptionDetails.ResponseStatusCode;
        httpContext.Response.ContentType = JsonContentType;

        var errorResponse = new ErrorResponse
        {
            ErrorId = exceptionDetails.ErrorId,
            ErrorCode = exceptionDetails.ErrorCode,
            Source = exceptionDetails.Source,
            Message = exceptionDetails.Message,
            Timestamp = exceptionDetails.Timestamp,
        };

        return httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);
    }

    private static ExceptionDetails CreateGeneralExceptionDetails(HttpContext httpContext, Exception exception)
    {
        return new ExceptionDetails
        {
            ErrorId = GenerateErrorId(),
            Timestamp = GetTimestamp(),
            RequestPath = httpContext.Request.Path.ToString(),
            Source = exception.Source,
            StackTrace = exception.StackTrace,
        };
    }

    private void LogError(ExceptionDetails exceptionDetails)
    {
        var errorMessage =
            $"""
            Error ID: {exceptionDetails.ErrorId};
            Timestamp: {exceptionDetails.Timestamp};
            Path: {exceptionDetails.RequestPath};
            Message: {exceptionDetails.Message};
            Source: {exceptionDetails.Source};
            Stacktrace: {exceptionDetails.StackTrace};
            """;

        this.logger.LogError(errorMessage);
    }

    private record ExceptionDetails
    {
        public Guid ErrorId { get; set; }

        public string ErrorCode { get; set; }

        public long Timestamp { get; set; }

        public string Message { get; set; }

        public string Source { get; set; }

        public string StackTrace { get; set; }

        public int ResponseStatusCode { get; set; }

        public string RequestPath { get; set; }
    }
}
