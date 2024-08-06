using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Core.Results;

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
    /// Generates response with status based on code in <see cref="ServiceResult"/>.
    /// </summary>
    /// <param name="serviceResult">Operation result.</param>
    /// <returns>Response object with status code matching in result.</returns>
    protected ObjectResult CreateBadResponseByServiceResult(ServiceResult serviceResult) =>
        serviceResult.Result switch
        {
            ServiceResultType.InvalidData => this.StatusCode(StatusCodes.Status400BadRequest, serviceResult.ErrorMessage),
            ServiceResultType.Unauthenticated => this.StatusCode(StatusCodes.Status401Unauthorized, serviceResult.ErrorMessage),
            ServiceResultType.Unauthorized => this.StatusCode(StatusCodes.Status403Forbidden, serviceResult.ErrorMessage),
            ServiceResultType.NotFound => this.StatusCode(StatusCodes.Status404NotFound, serviceResult.ErrorMessage),
            ServiceResultType.InternalError => this.StatusCode(StatusCodes.Status500InternalServerError, serviceResult.ErrorMessage),
            var _ => throw new NotImplementedException($"Not-supported {serviceResult.Result} operation result type to generate response."),
        };
}
