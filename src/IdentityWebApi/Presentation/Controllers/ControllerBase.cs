using IdentityWebApi.Core.Results;

using MediatR;

using System;

namespace IdentityWebApi.Presentation.Controllers;

/// <summary>
/// Base controller with reusable logic.
/// </summary>
[Microsoft.AspNetCore.Mvc.ApiController]
[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
public abstract class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
    /// <summary>
    /// <see cref="IMediator"/>.
    /// </summary>
    protected readonly IMediator Mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ControllerBase"/> class.
    /// </summary>
    // todo: remove later
    protected ControllerBase()
    {

    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ControllerBase"/> class.
    /// </summary>
    /// <param name="mediator"><see cref="IMediator"/>.</param>
    protected ControllerBase(IMediator mediator)
    {
        this.Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Generates response with status based on code in <see cref="ServiceResult"/>.
    /// </summary>
    /// <param name="serviceResult">Result of operation.</param>
    /// <returns>Response object with status code matching in result.</returns>
    protected Microsoft.AspNetCore.Mvc.ObjectResult CreateFailedResponseByServiceResult(ServiceResult serviceResult) =>
        this.StatusCode((int)serviceResult.Result, serviceResult.Message);
}