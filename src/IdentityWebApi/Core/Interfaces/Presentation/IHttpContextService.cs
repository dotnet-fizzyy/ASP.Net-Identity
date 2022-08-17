using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.Presentation;

/// <summary>
/// HTTP Context service.
/// </summary>
public interface IHttpContextService
{
    /// <summary>
    /// Signs in user using cookies authentication mechanism.
    /// </summary>
    /// <param name="user"><see cref="ClaimsPrincipal"/> user to perform authentication.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task SignInUsingCookiesAsync(ClaimsPrincipal user);

    /// <summary>
    /// Generates link to confirm email on registration process.
    /// </summary>
    /// <param name="email">Email to confirm.</param>
    /// <param name="token">Confirmation token.</param>
    /// <returns>Link to confirm email on registration process.</returns>
    string GenerateConfirmEmailLink(string email, string token);

    /// <summary>
    /// Generates link to get user profile action.
    /// </summary>
    /// <param name="id">User identifier.</param>
    /// <returns>Link to to get user profile action.</returns>
    string GenerateGetUserLink(Guid id);
}