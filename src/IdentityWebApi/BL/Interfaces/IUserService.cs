using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.PL.Models.Action;
using IdentityWebApi.PL.Models.DTO;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.BL.Interfaces;

public interface IUserService
{
    Task<ServiceResult<UserResultDto>> GetUserAsync(Guid id);

    Task<ServiceResult<UserResultDto>> CreateUserAsync(UserActionModel user);

    Task<ServiceResult<UserResultDto>> UpdateUserAsync(UserActionModel user);

    Task<ServiceResult> RemoveUserAsync(Guid id);
}
