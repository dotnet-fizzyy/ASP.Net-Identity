using IdentityWebApi.BL.ResultWrappers;

namespace IdentityWebApi.PL.Controllers;

[Microsoft.AspNetCore.Mvc.ApiController]
[Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
public abstract class ControllerBase : Microsoft.AspNetCore.Mvc.ControllerBase
{
    protected Microsoft.AspNetCore.Mvc.ObjectResult GetFailedResponseByServiceResult(ServiceResult serviceResult) => 
        StatusCode((int) serviceResult.Result, serviceResult.Message);
}