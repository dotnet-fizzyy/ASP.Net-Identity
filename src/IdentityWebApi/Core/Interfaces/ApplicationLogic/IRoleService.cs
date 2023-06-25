using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Results;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.ApplicationLogic;

/// <summary>
/// Role service.
/// </summary>
public interface IRoleService
{
    /// <summary>
    /// Grants role to user.
    /// </summary>
    /// <param name="roleDto"><see cref="UserRoleDto"/>.</param>
    /// <returns>A <see cref="ServiceResult"/> with operation result.</returns>
    Task<ServiceResult> GrantRoleToUserAsync(UserRoleDto roleDto);

    /// <summary>
    /// Revokes role from user.
    /// </summary>
    /// <param name="roleDto"><see cref="UserRoleDto"/>.</param>
    /// <returns>A <see cref="ServiceResult"/> with operation result.</returns>
    Task<ServiceResult> RevokeRoleFromUser(UserRoleDto roleDto);

    /// <summary>
    /// Creates role.
    /// </summary>
    /// <param name="roleDto"><see cref="RoleCreationDto"/>.</param>
    /// <returns>A <see cref="ServiceResult{T}"/> with created role.</returns>
    Task<ServiceResult<RoleDto>> CreateRoleAsync(RoleCreationDto roleDto);

    /// <summary>
    /// Updates role.
    /// </summary>
    /// <param name="roleDto"><see cref="RoleDto"/>.</param>
    /// <returns>A <see cref="ServiceResult{T}"/> with updated role.</returns>
    Task<ServiceResult<RoleDto>> UpdateRoleAsync(RoleDto roleDto);

    /// <summary>
    /// Removes role by id.
    /// </summary>
    /// <param name="id"><see cref="UserRoleDto"/>.</param>
    /// <returns>A <see cref="ServiceResult"/> with operation result.</returns>
    Task<ServiceResult> RemoveRoleAsync(Guid id);
}
