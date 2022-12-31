using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Results;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.Infrastructure;

/// <summary>
/// User repository.
/// </summary>
[Obsolete("Remove after CQRS pattern full implementation")]
public interface IUserRepository : IBaseRepository<AppUser>
{
    /// <summary>
    /// Gets user entity by id.
    /// </summary>
    /// <param name="id">User id.</param>
    /// <returns>A <see cref="ServiceResult{T}"/> with found user entity.</returns>
    Task<ServiceResult<AppUser>> GetUserWithRoles(Guid id);

    /// <summary>
    /// Performs user sign-in action.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="password">User password.</param>
    /// <returns>A <see cref="ServiceResult{T}"/> with sign-in user entity.</returns>
    Task<ServiceResult<AppUser>> SignInUserAsync(string email, string password);

    /// <summary>
    /// Confirms user email.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="token">Email confirmation token.</param>
    /// <returns>A <see cref="ServiceResult{T}"/> with user entity.</returns>
    Task<ServiceResult<AppUser>> ConfirmUserEmailAsync(string email, string token);

    /// <summary>
    /// Updates user entity without password.
    /// </summary>
    /// <param name="appUser"><see cref="AppUser"/> entity to update.</param>
    /// <returns>A <see cref="ServiceResult{T}"/> with updated user entity.</returns>
    Task<ServiceResult<AppUser>> UpdateUserAsync(AppUser appUser);

    /// <summary>
    /// Creates user entity.
    /// </summary>
    /// <param name="appUser"><see cref="AppUser"/> entity to create.</param>
    /// <param name="password">User password.</param>
    /// <param name="role">User role to assign.</param>
    /// <param name="shouldConfirmImmediately">Flag indicating whether email should be confirmed immediately.</param>
    /// <returns>A <see cref="ServiceResult{T}"/> with created user entity.</returns>
    Task<ServiceResult<(AppUser appUser, string token)>> CreateUserAsync(
        AppUser appUser,
        string password,
        string role,
        bool shouldConfirmImmediately);

    /// <summary>
    /// Removes user by id.
    /// </summary>
    /// <param name="id">User id.</param>
    /// <returns>A <see cref="ServiceResult"/> representing the operation result.</returns>
    Task<ServiceResult> RemoveUserAsync(Guid id);
}
