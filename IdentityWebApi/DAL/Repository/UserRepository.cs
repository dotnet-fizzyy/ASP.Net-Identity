using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            var existingUser = await _userManager.Users.FirstOrDefaultAsync(x => x.Id == appUser.Id);
            if (existingUser is null)
            {
                return new ServiceResult<AppUser>
                {
                    ServiceResultType = ServiceResultType.NotFound
                };
            }

            existingUser.UserName = appUser.UserName;
            existingUser.Email = appUser.Email;
            existingUser.PhoneNumber = appUser.PhoneNumber;
            existingUser.ConcurrencyStamp = appUser.ConcurrencyStamp;
            
            await _userManager.UpdateAsync(existingUser);

            return new ServiceResult<AppUser>
            {
                ServiceResultType = ServiceResultType.Success, 
                Data = existingUser
            };
        }

        public async Task<ServiceResult<AppUser>> CreateUserAsync(AppUser appUser, string password, string role)
        {
            if (!DatabaseUtilities.RoleExists(_appSettings.IdentitySettings.Roles, role))
            {
                return new ServiceResult<AppUser>
                {
                    ServiceResultType = ServiceResultType.InvalidData, 
                    Message = "No such role exists"
                };
            }

            var userCreationResult = await _userManager.CreateAsync(appUser, password);
            if (!userCreationResult.Succeeded)
            {
                return CreateErrorMessage(userCreationResult.Errors);
            }

            var roleAssignmentResult = await _userManager.AddToRoleAsync(appUser, role);
            if (!roleAssignmentResult.Succeeded)
            {
                return CreateErrorMessage(roleAssignmentResult.Errors);
            }

            return new ServiceResult<AppUser>
            {
                ServiceResultType = ServiceResultType.Success,
                Data = appUser
            };
        }

        private static ServiceResult<AppUser> CreateErrorMessage(IEnumerable<IdentityError> errors)
            => new()
            {
                ServiceResultType = ServiceResultType.InternalError, 
                Message = string.Join(",", errors.Select(e => e.Description))
            };
    }
}