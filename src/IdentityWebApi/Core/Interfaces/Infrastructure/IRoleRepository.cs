using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Results;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.Core.Interfaces.Infrastructure;

public interface IRoleRepository
{
    Task<ServiceResult<AppRole>> GetRoleByIdAsync(Guid id);

    Task<ServiceResult> GrantRoleToUserAsync(Guid userId, Guid roleId);

    Task<ServiceResult> RevokeRoleFromUserAsync(Guid userId, Guid roleId);

    Task<ServiceResult<AppRole>> CreateRoleAsync(AppRole entity);

    Task<ServiceResult<AppRole>> UpdateRoleAsync(AppRole entity);

    Task<ServiceResult> RemoveRoleAsync(Guid id);
}
