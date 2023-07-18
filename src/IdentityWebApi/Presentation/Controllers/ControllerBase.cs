using IdentityWebApi.Core.Results;

using MediatR;

using System;

using ApiController = Microsoft.AspNetCore.Mvc.ApiControllerAttribute;
using AspControllerBase = Microsoft.AspNetCore.Mvc.ControllerBase;
using ObjectResult = Microsoft.AspNetCore.Mvc.ObjectResult;
using Route = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace IdentityWebApi.Presentation.Controllers;

/// <summary>
/// Base controller with reusable logic.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class ControllerBase : AspControllerBase
{
    /// <summary>
    /// <see cref="IMediator"/>.
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
    /// <param name="serviceResult">Result of operation.</param>
    /// <returns>Response object with status code matching in result.</returns>
    protected ObjectResult CreateResponseByServiceResult(ServiceResult serviceResult) =>
        this.StatusCode((int)serviceResult.Result, serviceResult.Message);
}
