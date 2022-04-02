using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.PL.Models.DTO;

using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityWebApi.BL.Interfaces;

public interface IClaimsService
{
    ServiceResult<Guid> GetUserIdFromIdentityUser(ClaimsPrincipal user);

    ServiceResult<string> GetUserEmailFromIdentityUser(ClaimsPrincipal user);

    ServiceResult<IEnumerable<string>> GetUserRolesFromIdentityUser(ClaimsPrincipal user);

    ClaimsPrincipal AssignClaims(UserResultDto userDto);
}
