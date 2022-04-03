using IdentityWebApi.Core.Results;
using System;
using System.Threading.Tasks;
using IdentityWebApi.Presentation.Models.Action;
using IdentityWebApi.Presentation.Models.DTO;

namespace IdentityWebApi.Core.Interfaces.ApplicationLogic;

public interface IUserService
{
    Task<ServiceResult<UserResultDto>> GetUserAsync(Guid id);

    Task<ServiceResult<UserResultDto>> CreateUserAsync(UserActionModel user);

    Task<ServiceResult<UserResultDto>> UpdateUserAsync(UserActionModel user);

    Task<ServiceResult> RemoveUserAsync(Guid id);
}
