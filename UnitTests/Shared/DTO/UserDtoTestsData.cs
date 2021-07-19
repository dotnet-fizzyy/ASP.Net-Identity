using System;
using IdentityWebApi.PL.Models.Action;
using IdentityWebApi.PL.Models.DTO;
using UnitTests.Shared.Constants;

namespace UnitTests.Shared.DTO
{
    public static class UserDtoTestsData
    {
        public static UserActionModel GetUserActionDtoModel(Guid id, string userName = default, string userRole = default, string email = default)
            => new()
            {
                Id = id,
                UserName = userName ?? "UserName",
                UserRole = userRole ?? UserConstants.UserRoles[0],
                Password = "TestPassword",
                Email = email ?? UserConstants.UserEmail
            };
    }
}