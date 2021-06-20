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
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppSettings _appSettings;

        public UserRepository(
            DatabaseContext databaseContext, 
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            AppSettings appSettings
        ) : base(databaseContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings;
        }
        
        public async Task<ServiceResult<AppUser>> SignInUserAsync(string email, string password)
        {
            var appUser = await _userManager.FindByEmailAsync(email);
            if (appUser is null)
            {
                return new ServiceResult<AppUser>(ServiceResultType.InvalidData);
            }

            var authResult = await _signInManager.PasswordSignInAsync(appUser, password, false, false);
            if (!authResult.Succeeded)
            {
                return new ServiceResult<AppUser>(ServiceResultType.InvalidData);
            }

            await _signInManager.SignInAsync(appUser, false);

            return new ServiceResult<AppUser>(ServiceResultType.Success, appUser);
        }
        
        public async Task<ServiceResult<(AppUser appUser, string token)>> CreateUserAsync(AppUser appUser, string password, string role, bool confirmImmediately)
        {
            var token = string.Empty;
            var userCreationResult = await _userManager.CreateAsync(appUser, password);
            
            if (!userCreationResult.Succeeded)
            {
                return CreateInternalErrorMessageOnCreate(userCreationResult.Errors);
            }

            if (!string.IsNullOrWhiteSpace(role))
            {
                if (!DatabaseUtilities.RoleExists(_appSettings.IdentitySettings.Roles, role))
                {
                    return new ServiceResult<(AppUser appUser, string token)>(ServiceResultType.NotFound, "No such role exists");
                }
            
                var roleAssignmentResult = await _userManager.AddToRoleAsync(appUser, role);
                if (!roleAssignmentResult.Succeeded)
                {
                    return CreateInternalErrorMessageOnCreate(roleAssignmentResult.Errors);
                }
            }
            
            if (_appSettings.IdentitySettings.Email.RequireConfirmation)
            {
                token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);

                if (confirmImmediately)
                {
                    await _userManager.ConfirmEmailAsync(appUser, token);
                }
            }
            
            return new ServiceResult<(AppUser appUser, string token)>
            {
                ServiceResultType = ServiceResultType.Success,
                Data = (appUser, token)
            };
        }

        public async Task<ServiceResult<AppUser>> ConfirmUserEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return new ServiceResult<AppUser>(ServiceResultType.NotFound);
            }

            var confirmationResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!confirmationResult.Succeeded)
            {
                return CreateInternalErrorMessage(confirmationResult.Errors);
            }
            
            return new ServiceResult<AppUser>(ServiceResultType.Success);
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
        
        private static ServiceResult<(AppUser appUser, string token)> CreateInternalErrorMessageOnCreate(IEnumerable<IdentityError> errors)
            => new()
            {
                ServiceResultType = ServiceResultType.InternalError, 
                Message = string.Join(",", errors.Select(e => e.Description)),
            };
        
        private static ServiceResult<AppUser> CreateInternalErrorMessage(IEnumerable<IdentityError> errors)
            => new()
            {
                ServiceResultType = ServiceResultType.InternalError, 
                Message = string.Join(",", errors.Select(e => e.Description)),
            };
    }
}