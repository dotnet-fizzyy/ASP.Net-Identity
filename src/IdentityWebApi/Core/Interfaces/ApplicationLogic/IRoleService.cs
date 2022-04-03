using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Results;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.ApplicationLogic;

public interface IRoleService
{
    Task<ServiceResult<RoleDto>> GetRoleByIdAsync(Guid id);

    Task<ServiceResult> GrantRoleToUserAsync(UserRoleDto roleDto);

    Task<ServiceResult> RevokeRoleFromUser(UserRoleDto roleDto);

    Task<ServiceResult<RoleDto>> CreateRoleAsync(RoleCreationDto roleDto);

    Task<ServiceResult<RoleDto>> UpdateRoleAsync(RoleDto roleDto);

    Task<ServiceResult> RemoveRoleAsync(Guid id);
}
