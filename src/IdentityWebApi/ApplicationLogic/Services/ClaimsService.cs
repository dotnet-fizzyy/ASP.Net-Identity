using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Interfaces.ApplicationLogic;
using IdentityWebApi.Core.Results;

using Microsoft.AspNetCore.Authentication.Cookies;

using System;
using System.Linq;
using System.Security.Claims;

namespace IdentityWebApi.ApplicationLogic.Services;

/// <inheritdoc cref="IClaimsService" />
public class ClaimsService : IClaimsService
{
    /// <inheritdoc />
    public ServiceResult<Guid> GetUserIdFromIdentityUser(ClaimsPrincipal user)
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

    /// <inheritdoc />
    public ClaimsPrincipal AssignClaims(UserResultDto userDto)
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
