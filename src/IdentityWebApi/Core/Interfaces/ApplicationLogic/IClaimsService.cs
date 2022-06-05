using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Results;

using System;
using System.Security.Claims;

namespace IdentityWebApi.Core.Interfaces.ApplicationLogic;

/// <summary>
/// JWT claims service.
/// </summary>
public interface IClaimsService
{
    /// <summary>
    /// Gets user id from <see cref="ClaimsPrincipal"/> user.
    /// </summary>
    /// <param name="user"><see cref="ClaimsPrincipal"/>.</param>
    /// <returns><see cref="Guid"/> with user id.</returns>
    ServiceResult<Guid> GetUserIdFromIdentityUser(ClaimsPrincipal user);

    /// <summary>
    /// Creates <see cref="ClaimsPrincipal"/> to put in JWT.
    /// </summary>
    /// <param name="userDto"><see cref="UserResultDto"/>.</param>
    /// <returns><see cref="ClaimsPrincipal"/> to assign to JWT.</returns>
    ClaimsPrincipal AssignClaims(UserResultDto userDto);
}
