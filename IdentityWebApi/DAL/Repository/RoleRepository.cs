using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityWebApi.BL.Enums;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.DAL.Entities;
using IdentityWebApi.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityWebApi.DAL.Repository
{
    public class RoleRepository : BaseRepository<AppRole>, IRoleRepository
    {
        private const string ExistingRoleEntityExceptionMessage = "This role already exists";
        public const string MissingRoleExceptionMessage = "No such role exists";

        public RoleRepository(DatabaseContext databaseContext) : base(databaseContext) { }

        public async Task<ServiceResult<AppRole>> GetRoleByIdAsync(Guid id)
        {
            var appRole = await GetAppRole(id);
            if (appRole is null)
            {
                return new ServiceResult<AppRole>(ServiceResultType.NotFound);
            }

            return new ServiceResult<AppRole>(ServiceResultType.Success, appRole);
        }

        public async Task<ServiceResult> GrantRoleToUserAsync(Guid userId, Guid roleId)
        {
            var appRole = await GetAppRole(roleId);
            if (appRole is null)
            {
                return new ServiceResult<AppRole>(ServiceResultType.NotFound, MissingRoleExceptionMessage);
            }

            var appUser = await GetAppUser(userId);
            if (appUser is null)
            {
                return new ServiceResult<AppRole>(ServiceResultType.NotFound, UserRepository.MissingUserEntityExceptionMessage);
            }

            appUser.UserRoles.Add(new AppUserRole
            {
                Role = appRole,
                AppUser = appUser
            });

            return new ServiceResult(ServiceResultType.Success);
        }

        public async Task<ServiceResult> RevokeRoleFromUserAsync(Guid userId, Guid roleId)
        {
            var appRole = await GetAppRole(roleId);
            if (appRole is null)
            {
                return new ServiceResult<AppRole>(ServiceResultType.NotFound, MissingRoleExceptionMessage);
            }

            var appUser = await GetAppUser(userId);
            if (appUser is null)
            {
                return new ServiceResult<AppRole>(ServiceResultType.NotFound, UserRepository.MissingUserEntityExceptionMessage);
            }

            appUser.UserRoles.Remove(appUser.UserRoles.First(x => x.Role.Id == roleId && x.AppUser.Id == userId));

            return new ServiceResult(ServiceResultType.Success);
        }

        public async Task<ServiceResult<AppRole>> CreateRoleAsync(AppRole entity)
        {
            var appRole = await DatabaseContext.Roles.FirstOrDefaultAsync(x => x.Name == entity.Name);
            if (appRole is not null)
            {
                return new ServiceResult<AppRole>(ServiceResultType.InvalidData, ExistingRoleEntityExceptionMessage);
            }

            entity.ConcurrencyStamp = Guid.NewGuid().ToString();
            entity.CreationDate = DateTime.UtcNow;
            
            var roleCreationResult = await DatabaseContext.Roles.AddAsync(entity);
            
            return new ServiceResult<AppRole>(ServiceResultType.Success, roleCreationResult.Entity);
        }

        public async Task<ServiceResult<AppRole>> UpdateRoleAsync(AppRole entity)
        {
            var appRole = await GetAppRole(entity.Id);
            if (appRole is null)
            {
                return new ServiceResult<AppRole>(ServiceResultType.NotFound, MissingRoleExceptionMessage);
            }

            appRole.Name = entity.Name;
            appRole.ConcurrencyStamp = entity.ConcurrencyStamp;

            return new ServiceResult<AppRole>(ServiceResultType.Success, appRole);
        }

        public async Task<ServiceResult> RemoveRoleAsync(Guid id)
        {
            var appRole = await GetAppRole(id);
            if (appRole is null)
            {
                return new ServiceResult(ServiceResultType.NotFound, MissingRoleExceptionMessage);
            }

            DatabaseContext.Roles.Remove(appRole);
            
            return new ServiceResult(ServiceResultType.Success);
        }

        private async Task<AppUser> GetAppUser(Guid id) =>
            await DatabaseContext.Users
                .Include(x => x.UserRoles)
                .SingleOrDefaultAsync(x => x.Id == id);

        private async Task<AppRole> GetAppRole(Guid id) => 
            await DatabaseContext.Roles
                .SingleOrDefaultAsync(x => x.Id == id);
    }
}