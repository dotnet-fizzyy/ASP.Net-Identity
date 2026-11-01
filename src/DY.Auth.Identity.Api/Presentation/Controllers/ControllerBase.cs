using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Core.Results;
using DY.Auth.Identity.Api.Presentation.Models.Response;

using MediatR;

using Microsoft.AspNetCore.Http;

using System;

using ApiController = Microsoft.AspNetCore.Mvc.ApiControllerAttribute;
using AspControllerBase = Microsoft.AspNetCore.Mvc.ControllerBase;
using ObjectResult = Microsoft.AspNetCore.Mvc.ObjectResult;
using Route = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace DY.Auth.Identity.Api.Presentation.Controllers;

/// <summary>
/// Base controller with reusable logic.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class ControllerBase : AspControllerBase
{
    /// <summary>
    /// The instance of <see cref="IMediator"/>.
    /// </summary>
    protected readonly IMediator Mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ControllerBase"/> class.
    /// </summary>
    /// <param name="mediator">The instance of <see cref="IMediator"/>.</param>
    protected ControllerBase(IMediator mediator)
    {
        this.Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Generates an error response with a status based on the <see cref="ServiceResult"/>.
    /// </summary>
    /// <param name="serviceResult">Operation result.</param>
    /// <returns>An error response with status code matching the result.</returns>
    protected ObjectResult CreateErrorResponse(ServiceResult serviceResult)
    {
        ArgumentNullException.ThrowIfNull(serviceResult);

        if (!serviceResult.IsResultFailed)
        {
            throw new ArgumentException("Cannot create an error response for a successful result.", nameof(serviceResult));
        }

        var errorResponse = new ErrorResponse();

        switch (serviceResult.Result)
        {
            case ServiceResultType.InvalidData:
                errorResponse.StatusCode = StatusCodes.Status400BadRequest;
                errorResponse.ErrorCode = "INVALID_DATA";
                errorResponse.Message = serviceResult.ErrorMessage ?? "The request contains invalid data.";
                break;

            case ServiceResultType.Unauthenticated:
                errorResponse.StatusCode = StatusCodes.Status401Unauthorized;
                errorResponse.ErrorCode = "UNAUTHENTICATED";
                errorResponse.Message = serviceResult.ErrorMessage ?? "Authentication is required.";
                break;

            case ServiceResultType.Unauthorized:
                errorResponse.StatusCode = StatusCodes.Status403Forbidden;
                errorResponse.ErrorCode = "UNAUTHORIZED";
                errorResponse.Message = serviceResult.ErrorMessage ?? "You are not authorized to perform this operation.";
                break;

            case ServiceResultType.NotFound:
                errorResponse.StatusCode = StatusCodes.Status404NotFound;
                errorResponse.ErrorCode = "NOT_FOUND";
                errorResponse.Message = serviceResult.ErrorMessage ?? "The requested resource was not found.";
                break;

            case ServiceResultType.InternalError:
                errorResponse.StatusCode = StatusCodes.Status500InternalServerError;
                errorResponse.ErrorCode = "INTERNAL_SERVER_ERROR";
                errorResponse.Message = "An unexpected error occurred.";
                break;

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(serviceResult),
                    serviceResult.Result,
                    "Unsupported service result type.");
        }

        return this.StatusCode(errorResponse.StatusCode, errorResponse);
    }
}
