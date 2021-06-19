using System;
using System.Threading.Tasks;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.PL.Models.DTO;

namespace IdentityWebApi.BL.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResult<UserDto>> GetUserAsync(Guid id);
        
        Task<ServiceResult<UserDto>> CreateUserAsync(UserDto user);
        
        Task<ServiceResult<UserDto>> UpdateUserAsync(UserDto user);

        Task<ServiceResult> RemoveUserAsync(Guid id);
    }
}