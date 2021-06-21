using System;
using System.Security.Claims;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.PL.Models.DTO;

namespace IdentityWebApi.BL.Interfaces
{
    public interface IClaimsService
    {
        ServiceResult<Guid> GetUserIdFromIdentityUser(ClaimsIdentity user);
        
        ServiceResult<string> GetUserEmailFromIdentityUser(ClaimsIdentity user);
        
        ServiceResult<string> GetUserRoleFromIdentityUser(ClaimsIdentity user);

        ClaimsPrincipal AssignClaims(UserDto userDto);
    }
}