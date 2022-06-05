using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Interfaces.ApplicationLogic;
using IdentityWebApi.Core.Results;

using Microsoft.AspNetCore.Authentication.Cookies;

using System;
using System.Security.Claims;

namespace IdentityWebApi.ApplicationLogic.Services;

/// <inheritdoc cref="IClaimsService" />
public class ClaimsService : IClaimsService
{
    /// <inheritdoc />
    public ServiceResult<Guid> GetUserIdFromIdentityUser(ClaimsPrincipal user)
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var userId))
        {
            return new ServiceResult<Guid>(ServiceResultType.InvalidData);
        }

        return new ServiceResult<Guid>(ServiceResultType.Success, userId);
    }

    /// <inheritdoc />
    public ClaimsPrincipal AssignClaims(UserResultDto userDto)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
            new Claim(ClaimTypes.Email, userDto.Email),
            new Claim(ClaimTypes.Role, string.Join(",", userDto.Roles)),
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        return new ClaimsPrincipal(claimsIdentity);
    }
}
