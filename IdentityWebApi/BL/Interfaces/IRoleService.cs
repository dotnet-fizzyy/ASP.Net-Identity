using System;
using System.Threading.Tasks;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.PL.Models.Action;
using IdentityWebApi.PL.Models.DTO;

namespace IdentityWebApi.BL.Interfaces
{
    public interface IRoleService
    {
        Task<ServiceResult> GrantRoleToUserAsync(UserRoleActionModel roleActionModel);
        
        Task<ServiceResult> RevokeRoleFromUser(UserRoleActionModel roleActionModel);
        
        Task<ServiceResult<RoleDto>> CreateRoleAsync(RoleDto roleDto);
        
        Task<ServiceResult<RoleDto>> UpdateRoleAsync(RoleDto roleDto);
        
        Task<ServiceResult> RemoveRoleAsync(Guid id);
    }
}