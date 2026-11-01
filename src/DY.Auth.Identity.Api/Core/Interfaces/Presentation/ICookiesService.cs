using System.Security.Claims;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.Core.Interfaces.Presentation;

/// <summary>
/// Cookie service.
/// </summary>
public interface ICookiesService
{
    /// <summary>
    /// Signs in user using cookies authentication mechanism.
    /// </summary>
    /// <param name="user"><see cref="ClaimsPrincipal"/> user to perform authentication.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SignInUsingCookiesAsync(ClaimsPrincipal user);
}
