using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Results;

using Microsoft.AspNetCore.Authentication.Cookies;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace IdentityWebApi.Presentation.Services;

/// <summary>
/// User claims service.
/// </summary>
public static class ClaimsService
{
    /// <summary>
    /// Gets user id from <see cref="ClaimsPrincipal"/> user.
    /// </summary>
    /// <param name="user"><see cref="ClaimsPrincipal"/>.</param>
    /// <returns><see cref="Guid"/> with user id.</returns>
    public static ServiceResult<Guid> GetUserIdFromIdentityUser(ClaimsPrincipal user)
    {
        var userId = Guid.Empty;
        var idClaim = user.FindFirstValue(ClaimTypes.NameIdentifier);

        var isClaimInvalid = string.IsNullOrEmpty(idClaim) ||
                             !Guid.TryParse(idClaim, out userId);

        if (isClaimInvalid)
        {
            return new ServiceResult<Guid>(ServiceResultType.InvalidData);
        }

        return new ServiceResult<Guid>(ServiceResultType.Success, userId);
    }

    /// <summary>
    /// Creates <see cref="ClaimsPrincipal"/> to put in authorization user.
    /// </summary>
    /// <param name="userId">User identifier.</param>
    /// <param name="email">User email.</param>
    /// <param name="userRoles">A collection of user roles.</param>
    /// <returns><see cref="ClaimsPrincipal"/> to assign to authorization user.</returns>
    public static ClaimsPrincipal AssignClaims(Guid userId, string email, IEnumerable<string> userRoles)
    {
        var claimsUserRoles = userRoles != null && userRoles.Any()
                                    ? string.Join(",", userRoles)
                                    : string.Empty;

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Role, claimsUserRoles),
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        return new ClaimsPrincipal(claimsIdentity);
    }
}
