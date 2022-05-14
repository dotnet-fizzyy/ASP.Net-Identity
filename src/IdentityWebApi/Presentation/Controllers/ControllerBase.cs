using IdentityWebApi.Core.Results;

namespace IdentityWebApi.Presentation.Controllers;

/// <summary>
/// Base controller with reusable logic.
/// </summary>
[Microsoft.AspNetCore.Mvc.ApiController]
[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
public abstract class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
    /// <summary>
    /// Generates response with status based on code in <see cref="ServiceResult"/>.
    /// </summary>
    /// <param name="serviceResult">Result of operation.</param>
    /// <returns>Response object with status code matching in result.</returns>
    protected Microsoft.AspNetCore.Mvc.ObjectResult GetFailedResponseByServiceResult(ServiceResult serviceResult) =>
        this.StatusCode((int)serviceResult.Result, serviceResult.Message);
}