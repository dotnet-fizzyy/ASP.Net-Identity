using System;
using System.Threading.Tasks;
using IdentityWebApi.BL.Enums;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.DAL.Entities;
using IdentityWebApi.DAL.Interfaces;
using IdentityWebApi.DAL.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityWebApi.DAL.Repository
{
    public class RoleRepository : BaseRepository<AppRole>, IRoleRepository
    {
        private const string MissingRoleExceptionMessage = "No such role exists";
        
        private readonly RoleManager<AppRole> _roleManager;

        public RoleRepository(DatabaseContext databaseContext, RoleManager<AppRole> roleManager) : base(databaseContext)
        {
            _roleManager = roleManager;
        }

        public async Task<ServiceResult<AppRole>> CreateRoleAsync(AppRole entity)
        {
            var existingRole = await _roleManager.RoleExistsAsync(entity.Name);
            if (existingRole)
            {
                return new ServiceResult<AppRole>(ServiceResultType.InvalidData, "This role already exists");
            }

            entity.ConcurrencyStamp = Guid.NewGuid().ToString();
            
            var roleCreationResult = await _roleManager.CreateAsync(entity);
            if (!roleCreationResult.Succeeded)
            {
                return new ServiceResult<AppRole>(
                    ServiceResultType.InternalError, 
                    DatabaseUtilities.CreateErrorMessage(roleCreationResult.Errors)
                );
            }
            
            return new ServiceResult<AppRole>(ServiceResultType.Success, entity);
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

            await _roleManager.UpdateAsync(appRole);

            return new ServiceResult<AppRole>(ServiceResultType.Success, appRole);
        }

        public async Task<ServiceResult> RemoveRoleAsync(Guid id)
        {
            var appRole = await GetAppRole(id);
            if (appRole is null)
            {
                return new ServiceResult(ServiceResultType.NotFound, MissingRoleExceptionMessage);
            }

            var removeResult = await _roleManager.DeleteAsync(appRole);
            if (!removeResult.Succeeded)
            {
                return new ServiceResult(
                    ServiceResultType.InternalError, 
                    DatabaseUtilities.CreateErrorMessage(removeResult.Errors)
                );
            }

            return new ServiceResult(ServiceResultType.Success);
        }
        
        
        private async Task<AppRole> GetAppRole(Guid id) => 
            await _roleManager.Roles.SingleOrDefaultAsync(x => x.Id == id);
    }
}