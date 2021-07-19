using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.PL.Models.DTO;

namespace IdentityWebApi.BL.Interfaces
{
    public interface IClaimsService
    {
        ServiceResult<Guid> GetUserIdFromIdentityUser(ClaimsPrincipal user);
        
        ServiceResult<string> GetUserEmailFromIdentityUser(ClaimsPrincipal user);
        
        ServiceResult<IEnumerable<string>> GetUserRolesFromIdentityUser(ClaimsPrincipal user);

        ClaimsPrincipal AssignClaims(UserResultDto userDto);
    }
}