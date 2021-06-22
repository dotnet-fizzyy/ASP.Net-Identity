using System;
using System.Threading.Tasks;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.PL.Models.DTO;

namespace IdentityWebApi.BL.Interfaces
{
    public interface IRoleService
    {
        Task<ServiceResult<RoleDto>> CreateRoleAsync(RoleDto roleDto);
        
        Task<ServiceResult<RoleDto>> UpdateRoleAsync(RoleDto roleDto);
        
        Task<ServiceResult> RemoveRoleAsync(Guid id);
    }
}