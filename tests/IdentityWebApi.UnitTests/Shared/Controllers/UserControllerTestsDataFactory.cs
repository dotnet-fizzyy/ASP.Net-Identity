using DY.Auth.Identity.Api.Core.Utilities;

using System.Collections.Generic;
using System.Security.Claims;

namespace IdentityWebApi.UnitTests.Shared.Controllers;

public static class UserControllerTestsDataFactory
{
    public static ClaimsPrincipal GetUserIdentity(
        string userId = default,
        string email = default,
        string[] roles = default)
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

        if (!roles.IsNullOrEmpty())
        {
            userClaims.Add(new Claim(ClaimTypes.Role, string.Join(",", roles)));
        }

        return new ClaimsPrincipal(new ClaimsIdentity(userClaims));
    }
}
