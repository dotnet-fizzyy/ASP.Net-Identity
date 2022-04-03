using IdentityWebApi.Core.Results;
using System;
using System.Threading.Tasks;
using IdentityWebApi.Presentation.Models.Action;
using IdentityWebApi.Presentation.Models.DTO;

namespace IdentityWebApi.Core.Interfaces.ApplicationLogic;

public interface IRoleService
{
    Task<ServiceResult<RoleDto>> GetRoleByIdAsync(Guid id);

    Task<ServiceResult> GrantRoleToUserAsync(UserRoleActionModel roleActionModel);

    Task<ServiceResult> RevokeRoleFromUser(UserRoleActionModel roleActionModel);

    Task<ServiceResult<RoleDto>> CreateRoleAsync(RoleCreationActionModel roleDto);

    Task<ServiceResult<RoleDto>> UpdateRoleAsync(RoleDto roleDto);

    Task<ServiceResult> RemoveRoleAsync(Guid id);
}
