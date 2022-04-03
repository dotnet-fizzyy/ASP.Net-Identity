using IdentityWebApi.ApplicationLogic.Models.Action;
using IdentityWebApi.Core.Results;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.ApplicationLogic;

public interface IUserService
{
    Task<ServiceResult<UserResultDto>> GetUserAsync(Guid id);

    Task<ServiceResult<UserResultDto>> CreateUserAsync(UserDto user);

    Task<ServiceResult<UserResultDto>> UpdateUserAsync(UserDto user);

    Task<ServiceResult> RemoveUserAsync(Guid id);
}
