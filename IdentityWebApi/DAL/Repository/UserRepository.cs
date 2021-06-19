using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityWebApi.BL.Constants;
using IdentityWebApi.BL.Enums;
using IdentityWebApi.BL.ResultWrappers;
using IdentityWebApi.DAL.Entities;
using IdentityWebApi.DAL.Interfaces;
using IdentityWebApi.DAL.Utilities;
using IdentityWebApi.Startup.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IdentityWebApi.DAL.Repository
{
    public class UserRepository : BaseRepository<AppUser>, IUserRepository
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppSettings _appSettings;

        public UserRepository(DatabaseContext databaseContext, UserManager<AppUser> userManager, AppSettings appSettings) : base(databaseContext)
        {
            _userManager = userManager;
            _appSettings = appSettings;
        }

        public async Task<ServiceResult<AppUser>> UpdateUserAsync(AppUser appUser)
        {
            var existingUser = await GetUserWithChildren(appUser.Id);
            if (existingUser is null)
            {
                return new ServiceResult<AppUser>(ServiceResultType.NotFound, ExceptionMessageConstants.MissingUser);
            }

            existingUser.UserName = appUser.UserName;
            existingUser.Email = appUser.Email;
            existingUser.PhoneNumber = appUser.PhoneNumber;
            existingUser.ConcurrencyStamp = appUser.ConcurrencyStamp;
            
            var updateResult = await _userManager.UpdateAsync(existingUser);
            if (!updateResult.Succeeded)
            {
                return CreateInternalErrorMessage(updateResult.Errors);
            }
            
            return new ServiceResult<AppUser>
            {
                ServiceResultType = ServiceResultType.Success, 
                Data = existingUser
            };
        }

        public async Task<ServiceResult<AppUser>> CreateUserAsync(AppUser appUser, string password, string role)
        {
            var userCreationResult = await _userManager.CreateAsync(appUser, password);
            if (!userCreationResult.Succeeded)
            {
                return CreateInternalErrorMessage(userCreationResult.Errors);
            }

            if (!string.IsNullOrWhiteSpace(role))
            {
                if (!DatabaseUtilities.RoleExists(_appSettings.IdentitySettings.Roles, role))
                {
                    return new ServiceResult<AppUser>(ServiceResultType.NotFound, "No such role exists");
                }
            
                var roleAssignmentResult = await _userManager.AddToRoleAsync(appUser, role);
                if (!roleAssignmentResult.Succeeded)
                {
                    return CreateInternalErrorMessage(roleAssignmentResult.Errors);
                }
            }

            return new ServiceResult<AppUser>
            {
                ServiceResultType = ServiceResultType.Success,
                Data = appUser
            };
        }

        public async Task<ServiceResult> RemoveUserAsync(Guid id)
        {
            var user = await GetUserWithChildren(id);
            if (user is null)
            {
                return new ServiceResult(ServiceResultType.NotFound, ExceptionMessageConstants.MissingUser);
            }

            var userRoles = user.UserRoles.Select(x => x.AppRole.Name);
            
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.DeleteAsync(user);

            return new ServiceResult(ServiceResultType.Success);
        }

        
        private async Task<AppUser> GetUserWithChildren(Guid id) 
            => await _userManager.Users
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.AppRole)
            .FirstOrDefaultAsync(x => x.Id == id);
        
        private static ServiceResult<AppUser> CreateInternalErrorMessage(IEnumerable<IdentityError> errors)
            => new()
            {
                ServiceResultType = ServiceResultType.InternalError, 
                Message = string.Join(",", errors.Select(e => e.Description))
            };
    }
}