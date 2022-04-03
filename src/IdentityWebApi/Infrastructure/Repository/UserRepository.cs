using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Constants;
using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Interfaces.Infrastructure;
using IdentityWebApi.Core.Results;
using IdentityWebApi.Startup.Settings;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IdentityWebApi.Infrastructure.Repository;

public class UserRepository : BaseRepository<AppUser>, IUserRepository
{
    public const string MissingUserEntityExceptionMessage = "No such user exists";

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

    public async Task<ServiceResult<AppUser>> GetUserWithRoles(Guid id)
    {
        var appUser = await GetUserWithChildren(x => x.Id == id);
        if (appUser is null)
        {
            return new ServiceResult<AppUser>(ServiceResultType.NotFound, ExceptionMessageConstants.InvalidAuthData);
        }

        return new ServiceResult<AppUser>(ServiceResultType.Success, appUser);
    }

    public async Task<ServiceResult<AppUser>> SignInUserAsync(string email, string password)
    {
        var appUser = await GetUserWithChildren(x => x.Email.ToLower() == email.ToLower());
        if (appUser is null)
        {
            return new ServiceResult<AppUser>(ServiceResultType.InvalidData, ExceptionMessageConstants.InvalidAuthData);
        }

        var authResult = await _signInManager.CheckPasswordSignInAsync(appUser, password, false);
        if (!authResult.Succeeded)
        {
            return new ServiceResult<AppUser>(ServiceResultType.InvalidData, ExceptionMessageConstants.InvalidAuthData);
        }

        return new ServiceResult<AppUser>(ServiceResultType.Success, appUser);
    }

    public async Task<ServiceResult<(AppUser appUser, string token)>> CreateUserAsync(AppUser appUser, string password,
        string role, bool shouldConfirmImmediately)
    {
        var token = string.Empty;
        var userCreationResult = await _userManager.CreateAsync(appUser, password);

        if (!userCreationResult.Succeeded)
        {
            return new ServiceResult<(AppUser appUser, string token)>(
                ServiceResultType.InternalError, 
                CreateErrorMessage(userCreationResult.Errors)
            );
        }

        if (!string.IsNullOrWhiteSpace(role))
        {
            if (!RoleExists(_appSettings.IdentitySettings.Roles, role))
            {
                return new ServiceResult<(AppUser appUser, string token)>(ServiceResultType.NotFound,
                    RoleRepository.MissingRoleExceptionMessage);
            }

            var roleAssignmentResult = await _userManager.AddToRoleAsync(appUser, role);
            if (!roleAssignmentResult.Succeeded)
            {
                return new ServiceResult<(AppUser appUser, string token)>(
                    ServiceResultType.InternalError, 
                    CreateErrorMessage(roleAssignmentResult.Errors)
                );
            }
        }

        if (_appSettings.IdentitySettings.Email.RequireConfirmation)
        {
            token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);

            if (shouldConfirmImmediately)
            {
                await _userManager.ConfirmEmailAsync(appUser, token);
            }
        }

        return new ServiceResult<(AppUser appUser, string token)>
        {
            Result = ServiceResultType.Success,
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
            return new ServiceResult<AppUser>(
                ServiceResultType.InternalError, 
                CreateErrorMessage(confirmationResult.Errors)
            );
        }

        return new ServiceResult<AppUser>(ServiceResultType.Success);
    }

    public async Task<ServiceResult<AppUser>> UpdateUserAsync(AppUser appUser)
    {
        var existingUser = await GetUserWithChildren(x => x.Id == appUser.Id);
        if (existingUser is null)
        {
            return new ServiceResult<AppUser>(ServiceResultType.NotFound, ExceptionMessageConstants.MissingUser);
        }

        existingUser.UserName = appUser.UserName;
        existingUser.Email = appUser.Email;
        existingUser.PhoneNumber = appUser.PhoneNumber;
        existingUser.ConcurrencyStamp = appUser.ConcurrencyStamp;

        return new ServiceResult<AppUser>
        {
            Result = ServiceResultType.Success,
            Data = existingUser
        };
    }

    public async Task<ServiceResult> RemoveUserAsync(Guid id)
    {
        var existingUser = await GetUserWithChildren(x => x.Id == id);
        if (existingUser is null)
        {
            return new ServiceResult(ServiceResultType.NotFound, ExceptionMessageConstants.MissingUser);
        }

        DatabaseContext.UserRoles.RemoveRange(existingUser.UserRoles);
        DatabaseContext.Users.Remove(existingUser);

        return new ServiceResult(ServiceResultType.Success);
    }


    private async Task<AppUser> GetUserWithChildren(Expression<Func<AppUser, bool>> expression)
        => await DatabaseContext.Users
            .AsNoTracking()
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .FirstOrDefaultAsync(expression);
    
    private static bool RoleExists(IEnumerable<string> roles, string role) =>
        roles.Any(x => string.Equals(x, role, StringComparison.CurrentCultureIgnoreCase));
    
    private static string CreateErrorMessage(IEnumerable<IdentityError> errors) =>
        string.Join(",", errors.Select(x => x.Description));
}
