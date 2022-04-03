using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Results;

using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityWebApi.Core.Interfaces.ApplicationLogic;

public interface IClaimsService
{
    ServiceResult<Guid> GetUserIdFromIdentityUser(ClaimsPrincipal user);

    ServiceResult<string> GetUserEmailFromIdentityUser(ClaimsPrincipal user);

    ServiceResult<IEnumerable<string>> GetUserRolesFromIdentityUser(ClaimsPrincipal user);

    ClaimsPrincipal AssignClaims(UserResultDto userDto);
}
