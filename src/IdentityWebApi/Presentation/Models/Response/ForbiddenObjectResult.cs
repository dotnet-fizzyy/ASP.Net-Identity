using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityWebApi.Presentation.Models.Response;

internal class ForbiddenObjectResult : ObjectResult
{
    public ForbiddenObjectResult(object value) : base(value)
    {
        StatusCode = StatusCodes.Status403Forbidden;
        Value = value;
    }
}