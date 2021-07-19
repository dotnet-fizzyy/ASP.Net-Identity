using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using IdentityWebApi.BL.Enums;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.PL.Models.DTO;

namespace UnitTests.Shared.Controllers
{
    public static class UserControllerTestsDataFactory
    {
        public static ServiceResult<UserResultDto> GetUserResult(
            ServiceResultType serviceResultType, 
            Guid userId = new(), 
            string message = null, 
            IEnumerable<string> userRoles = null
        )
            => new (serviceResultType, message, new UserResultDto
            {
                Id = userId,
                Roles = userRoles
            });
        
        public static ClaimsPrincipal GetUserIdentity(
            string userId = default, 
            string email = default, 
            string[] roles = default
        )
        {
            var userClaims = new List<Claim>();
            
            if (!string.IsNullOrWhiteSpace(userId))
            {
                userClaims.Add(new Claim(ClaimTypes.NameIdentifier, userId));
            }
            
            if (!string.IsNullOrWhiteSpace(email))
            {
                userClaims.Add(new Claim(ClaimTypes.Email, email));
            }
            
            if (roles != null && roles.Any())
            {
                userClaims.Add(new Claim(ClaimTypes.Role, string.Join(",", roles)));
            }
            
            return new ClaimsPrincipal(new ClaimsIdentity(userClaims));
        }
    }
}