using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityWebApi.Presentation.Models.Response;

/// <summary>
/// Custom wrapper for <see cref="ObjectResult"/> with Forbidden status.
/// </summary>
internal class ForbiddenObjectResult : ObjectResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ForbiddenObjectResult"/> class.
    /// </summary>
    /// <param name="value">Error message.</param>
    public ForbiddenObjectResult(object value)
        : base(value)
    {
        this.StatusCode = StatusCodes.Status403Forbidden;
        this.Value = value;
    }
}
