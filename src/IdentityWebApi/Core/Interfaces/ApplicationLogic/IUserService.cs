using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Results;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.ApplicationLogic;

/// <summary>
/// User service.
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Gets user by id.
    /// </summary>
    /// <param name="id">User id.</param>
    /// <returns>A <see cref="ServiceResult{T}"/> with user.</returns>
    Task<ServiceResult<UserResultDto>> GetUserAsync(Guid id);

    /// <summary>
    /// Creates user.
    /// </summary>
    /// <param name="user"><see cref="UserDto"/>.</param>
    /// <returns>A <see cref="ServiceResult{T}"/> with created user.</returns>
    Task<ServiceResult<UserResultDto>> CreateUserAsync(UserDto user);

    /// <summary>
    /// Updates user.
    /// </summary>
    /// <param name="user"><see cref="UserDto"/>.</param>
    /// <returns>A <see cref="ServiceResult{T}"/> with updated user.</returns>
    Task<ServiceResult<UserResultDto>> UpdateUserAsync(UserDto user);

    /// <summary>
    /// Removes user by id.
    /// </summary>
    /// <param name="id">User id.</param>
    /// <returns>A <see cref="ServiceResult"/>.</returns>
    Task<ServiceResult> RemoveUserAsync(Guid id);
}
