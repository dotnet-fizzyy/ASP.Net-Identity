using System;
using System.Threading.Tasks;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.DAL.Entities;

namespace IdentityWebApi.DAL.Interfaces
{
    public interface IRoleRepository
    {
        Task<ServiceResult<AppRole>> CreateRoleAsync(AppRole entity);
        
        Task<ServiceResult<AppRole>> UpdateRoleAsync(AppRole entity);
        
        Task<ServiceResult> RemoveRoleAsync(Guid id);
    }
}