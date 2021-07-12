using System;
using System.Threading.Tasks;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.PL.Models.DTO;

namespace IdentityWebApi.BL.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<UserResultDto>> GetUserAsync(Guid id);
        
        Task<ServiceResult<UserResultDto>> CreateUserAsync(UserActionDto user);
        
        Task<ServiceResult<UserResultDto>> UpdateUserAsync(UserActionDto user);

        Task<ServiceResult> RemoveUserAsync(Guid id);
    }
}