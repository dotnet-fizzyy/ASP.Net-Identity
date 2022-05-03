using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Results;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.Infrastructure;

public interface IUserRepository : IBaseRepository<AppUser>
{
    Task<ServiceResult<AppUser>> GetUserWithRoles(Guid id);

    Task<ServiceResult<AppUser>> SignInUserAsync(string email, string password);

    Task<ServiceResult<AppUser>> ConfirmUserEmailAsync(string email, string token);

    Task<ServiceResult<AppUser>> UpdateUserAsync(AppUser appUser);

    Task<ServiceResult<(AppUser appUser, string token)>> CreateUserAsync(
        AppUser appUser,
        string password,
        string role,
        bool shouldConfirmImmediately
    );

    Task<ServiceResult> RemoveUserAsync(Guid id);
}
