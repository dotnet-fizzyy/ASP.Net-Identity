using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Interfaces.Infrastructure;
using IdentityWebApi.Core.Results;

using Microsoft.EntityFrameworkCore;

using System;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWebApi.Infrastructure.Database.Repository;

/// <inheritdoc cref="IRoleRepository" />
[Obsolete("Remove after CQRS pattern full implementation")]
public class RoleRepository : BaseRepository<AppRole>, IRoleRepository
{
    /// <summary>
    /// Co.
    /// </summary>
    public const string MissingRoleExceptionMessage = "No such role exists";

    private const string ExistingRoleEntityExceptionMessage = "This role already exists";

    /// <summary>
    /// Initializes a new instance of the <see cref="RoleRepository"/> class.
    /// </summary>
    /// <param name="databaseContext"><see cref="DatabaseContext"/>.</param>
    public RoleRepository(DatabaseContext databaseContext)
        : base(databaseContext)
    {
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<AppRole>> GetRoleByIdAsync(Guid id)
    {
        var appRole = await this.GetAppRole(id);
        if (appRole is null)
        {
            return new ServiceResult<AppRole>(ServiceResultType.NotFound);
        }

        return new ServiceResult<AppRole>(ServiceResultType.Success, appRole);
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> GrantRoleToUserAsync(Guid userId, Guid roleId)
    {
        var appRole = await this.GetAppRole(roleId);
        if (appRole is null)
        {
            return new ServiceResult<AppRole>(ServiceResultType.NotFound, MissingRoleExceptionMessage);
        }

        var appUser = await this.GetAppUser(userId);
        if (appUser is null)
        {
            return new ServiceResult<AppRole>(
                ServiceResultType.NotFound,
                UserRepository.MissingUserEntityExceptionMessage
            );
        }

        appUser.UserRoles.Add(new AppUserRole
        {
            Role = appRole,
            AppUser = appUser,
        });

        return new ServiceResult(ServiceResultType.Success);
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> RevokeRoleFromUserAsync(Guid userId, Guid roleId)
    {
        var appRole = await this.GetAppRole(roleId);
        if (appRole is null)
        {
            return new ServiceResult<AppRole>(ServiceResultType.NotFound, MissingRoleExceptionMessage);
        }

        var appUser = await this.GetAppUser(userId);
        if (appUser is null)
        {
            return new ServiceResult<AppRole>(
                ServiceResultType.NotFound,
                UserRepository.MissingUserEntityExceptionMessage
            );
        }

        appUser.UserRoles.Remove(appUser.UserRoles.First(x => x.Role.Id == roleId && x.AppUser.Id == userId));

        return new ServiceResult(ServiceResultType.Success);
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<AppRole>> CreateRoleAsync(AppRole entity)
    {
        var appRole = await this.DatabaseContext.Roles.FirstOrDefaultAsync(x => x.Name == entity.Name);
        if (appRole is not null)
        {
            return new ServiceResult<AppRole>(ServiceResultType.InvalidData, ExistingRoleEntityExceptionMessage);
        }

        entity.ConcurrencyStamp = Guid.NewGuid().ToString();
        entity.CreationDate = DateTime.UtcNow;

        var roleCreationResult = await this.DatabaseContext.Roles.AddAsync(entity);

        return new ServiceResult<AppRole>(ServiceResultType.Success, roleCreationResult.Entity);
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<AppRole>> UpdateRoleAsync(AppRole entity)
    {
        var appRole = await this.GetAppRole(entity.Id);
        if (appRole is null)
        {
            return new ServiceResult<AppRole>(ServiceResultType.NotFound, MissingRoleExceptionMessage);
        }

        appRole.Name = entity.Name;
        appRole.ConcurrencyStamp = entity.ConcurrencyStamp;

        return new ServiceResult<AppRole>(ServiceResultType.Success, appRole);
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> RemoveRoleAsync(Guid id)
    {
        var appRole = await this.GetAppRole(id);
        if (appRole is null)
        {
            return new ServiceResult(ServiceResultType.NotFound, MissingRoleExceptionMessage);
        }

        this.DatabaseContext.Roles.Remove(appRole);

        return new ServiceResult(ServiceResultType.Success);
    }

    private async Task<AppUser> GetAppUser(Guid id) =>
        await this.DatabaseContext.Users
            .Include(x => x.UserRoles)
            .SingleOrDefaultAsync(x => x.Id == id);

    private async Task<AppRole> GetAppRole(Guid id) =>
        await this.DatabaseContext.Roles
            .SingleOrDefaultAsync(x => x.Id == id);
}
