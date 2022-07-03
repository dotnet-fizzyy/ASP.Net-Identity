using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Results;

using Microsoft.AspNetCore.Authentication.Cookies;

using System;
using System.Linq;
using System.Security.Claims;

namespace IdentityWebApi.Presentation.Services;

/// <summary>
/// User claims service.
/// </summary>
public class ClaimsService
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
    /// <param name="userDto"><see cref="UserResultDto"/>.</param>
    /// <returns><see cref="ClaimsPrincipal"/> to assign to authorization user.</returns>
    public static ClaimsPrincipal AssignClaims(UserResultDto userDto)
    {
        var userClaims = userDto.Roles != null && userDto.Roles.Any()
                                ? string.Join(",", userDto.Roles)
                                : string.Empty;

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
            new Claim(ClaimTypes.Email, userDto.Email),
            new Claim(ClaimTypes.Role, userClaims),
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        return new ClaimsPrincipal(claimsIdentity);
    }
}
