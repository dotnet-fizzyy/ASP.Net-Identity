using System;
using System.Linq;
using System.Security.Claims;
using IdentityWebApi.BL.Enums;
using IdentityWebApi.BL.Interfaces;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.PL.Models.DTO;

namespace IdentityWebApi.BL.Services
{
    public class ClaimsService : IClaimsService
    {
        public ServiceResult<Guid> GetUserIdFromIdentityUser(ClaimsIdentity user)
        {
            var id = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(id) || !Guid.TryParse(id, out var userId))
            {
                return new ServiceResult<Guid>(ServiceResultType.InvalidData);
            }

            return new ServiceResult<Guid>(ServiceResultType.Success, userId);
        }

        public ServiceResult<string> GetUserEmailFromIdentityUser(ClaimsIdentity user)
        {
            var email = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return new ServiceResult<string>(ServiceResultType.InvalidData);
            }

            return new ServiceResult<string>(ServiceResultType.Success, email);
        }
        
        public ServiceResult<string> GetUserRoleFromIdentityUser(ClaimsIdentity user)
        {
            var role = user.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(role))
            {
                return new ServiceResult<string>(ServiceResultType.InvalidData);
            }

            return new ServiceResult<string>(ServiceResultType.Success, role);
        }

        public ClaimsPrincipal AssignClaims(UserDto userDto)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
                new Claim(ClaimTypes.Email, userDto.Email),
                new Claim(ClaimTypes.Role, userDto.UserRole)
            };

            var claimsIdentity = new ClaimsIdentity(claims);

            return new ClaimsPrincipal(claimsIdentity);
        }
    }
}