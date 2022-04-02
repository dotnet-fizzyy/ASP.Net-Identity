using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.PL.Models.Action;
using IdentityWebApi.PL.Models.DTO;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.BL.Interfaces;

public interface IRoleService
{
    Task<ServiceResult<RoleDto>> GetRoleByIdAsync(Guid id);

    Task<ServiceResult> GrantRoleToUserAsync(UserRoleActionModel roleActionModel);

    Task<ServiceResult> RevokeRoleFromUser(UserRoleActionModel roleActionModel);

    Task<ServiceResult<RoleDto>> CreateRoleAsync(RoleCreationActionModel roleDto);

    Task<ServiceResult<RoleDto>> UpdateRoleAsync(RoleDto roleDto);

    Task<ServiceResult> RemoveRoleAsync(Guid id);
}
