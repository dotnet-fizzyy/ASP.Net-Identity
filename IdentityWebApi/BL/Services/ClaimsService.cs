using System;
using System.Security.Claims;
using IdentityWebApi.BL.Enums;
using IdentityWebApi.BL.Interfaces;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.PL.Models.DTO;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace IdentityWebApi.BL.Services
{
    public class ClaimsService : IClaimsService
    {
        public ServiceResult<Guid> GetUserIdFromIdentityUser(ClaimsPrincipal user)
        {
            var id = user.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var userId))
            {
                return new ServiceResult<Guid>(ServiceResultType.InvalidData);
            }

            return new ServiceResult<Guid>(ServiceResultType.Success, userId);
        }

        public ServiceResult<string> GetUserEmailFromIdentityUser(ClaimsPrincipal user)
        {
            var email = user.FindFirstValue(ClaimTypes.Email);

            return new ServiceResult<string>(string.IsNullOrEmpty(email) 
                ? ServiceResultType.InvalidData 
                : ServiceResultType.Success, 
                email);
        }
        
        public ServiceResult<string> GetUserRoleFromIdentityUser(ClaimsPrincipal user)
        {
            var role = user.FindFirstValue(ClaimTypes.Role);
            
            return new ServiceResult<string>(string.IsNullOrEmpty(role) 
                    ? ServiceResultType.InvalidData 
                    : ServiceResultType.Success, 
                role);
        }

        public ClaimsPrincipal AssignClaims(UserResultDto userDto)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
                new Claim(ClaimTypes.Email, userDto.Email),
                new Claim(ClaimTypes.Role, string.Join(",", userDto.Roles))
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            return new ClaimsPrincipal(claimsIdentity);
        }
    }
}