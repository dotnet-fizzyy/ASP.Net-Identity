using System.Security.Claims;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.User.Commands.AuthenticateUser;

/// <summary>
/// Authenticate user result model.
/// </summary>
public record AuthenticateUserResult
{
    /// <summary>
    /// Gets authenticated user principal.
    /// </summary>
    public ClaimsPrincipal AuthenticatedUser { get; init; }
}
