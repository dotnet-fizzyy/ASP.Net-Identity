using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Results;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.Infrastructure;

/// <summary>
/// Repository with functionality for AppRole entity.
/// </summary>
public interface IRoleRepository
{
    /// <summary>
    /// Searching for AppRole by identifier.
    /// </summary>
    /// <param name="id">Role identifier (PK).</param>
    /// <returns>A <see cref="Task{ServiceResult}"/> representing the result of the asynchronous operation.</returns>
    Task<ServiceResult<AppRole>> GetRoleByIdAsync(Guid id);

    /// <summary>
    /// Grant role to user.
    /// </summary>
    /// <param name="userId">User identifier (PK).</param>
    /// <param name="roleId">Role identifier (PK).</param>
    /// <returns>A <see cref="Task{ServiceResult}"/> representing the result of the asynchronous operation.</returns>
    Task<ServiceResult> GrantRoleToUserAsync(Guid userId, Guid roleId);

    /// <summary>
    /// Revokes role from user.
    /// </summary>
    /// <param name="userId">User identifier (PK).</param>
    /// <param name="roleId">Role identifier (PK).</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    Task<ServiceResult> RevokeRoleFromUserAsync(Guid userId, Guid roleId);

    /// <summary>
    /// Create new AppRole entity.
    /// </summary>
    /// <param name="entity"><see cref="AppRole"/>.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    Task<ServiceResult<AppRole>> CreateRoleAsync(AppRole entity);

    /// <summary>
    /// Update existing AppRole entity.
    /// </summary>
    /// <param name="entity"><see cref="AppRole"/>.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    Task<ServiceResult<AppRole>> UpdateRoleAsync(AppRole entity);

    /// <summary>
    /// Remove existing AppRole entity.
    /// </summary>
    /// <param name="id">Role identifier (PK).</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
    Task<ServiceResult> RemoveRoleAsync(Guid id);
}
