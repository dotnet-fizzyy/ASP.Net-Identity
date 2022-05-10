using IdentityWebApi.Core.ApplicationSettings;
using IdentityWebApi.Core.Constants;
using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Enums;
using IdentityWebApi.Core.Interfaces.Infrastructure;
using IdentityWebApi.Core.Results;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IdentityWebApi.Infrastructure.Database.Repository;

/// <inheritdoc cref="IUserRepository"/>
public class UserRepository : BaseRepository<AppUser>, IUserRepository
{
    /// <summary>
    /// Gets missing user exception message.
    /// </summary>
    public const string MissingUserEntityExceptionMessage = "No such user exists";

    private readonly UserManager<AppUser> userManager;
    private readonly SignInManager<AppUser> signInManager;
    private readonly AppSettings appSettings;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="databaseContext"><see cref="DatabaseContext"/>.</param>
    /// <param name="userManager"><see cref="UserManager{T}"/>.</param>
    /// <param name="signInManager"><see cref="SignInManager{T}"/>.</param>
    /// <param name="appSettings"><see cref="AppSettings"/>.</param>
    public UserRepository(
        DatabaseContext databaseContext,
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        AppSettings appSettings)
        : base(databaseContext)
    {
        this.userManager = userManager;
        this.signInManager = signInManager;
        this.appSettings = appSettings;
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<AppUser>> GetUserWithRoles(Guid id)
    {
        var appUser = await this.GetUserWithChildren(x => x.Id == id);
        if (appUser is null)
        {
            return new ServiceResult<AppUser>(ServiceResultType.NotFound, ExceptionMessageConstants.InvalidAuthData);
        }

        return new ServiceResult<AppUser>(ServiceResultType.Success, appUser);
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<AppUser>> SignInUserAsync(string email, string password)
    {
        var appUser = await this.GetUserWithChildren(x => x.Email.ToLower() == email.ToLower());
        if (appUser is null)
        {
            return new ServiceResult<AppUser>(ServiceResultType.InvalidData, ExceptionMessageConstants.InvalidAuthData);
        }

        var authResult = await this.signInManager.CheckPasswordSignInAsync(appUser, password, false);
        if (!authResult.Succeeded)
        {
            return new ServiceResult<AppUser>(ServiceResultType.InvalidData, ExceptionMessageConstants.InvalidAuthData);
        }

        return new ServiceResult<AppUser>(ServiceResultType.Success, appUser);
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<(AppUser appUser, string token)>> CreateUserAsync(
        AppUser appUser,
        string password,
        string role,
        bool shouldConfirmImmediately
    )
    {
        var token = string.Empty;
        var userCreationResult = await this.userManager.CreateAsync(appUser, password);

        if (!userCreationResult.Succeeded)
        {
            return new ServiceResult<(AppUser appUser, string token)>(
                ServiceResultType.InternalError,
                CreateErrorMessage(userCreationResult.Errors)
            );
        }

        if (!string.IsNullOrWhiteSpace(role))
        {
            if (!RoleExists(this.appSettings.IdentitySettings.Roles, role))
            {
                return new ServiceResult<(AppUser appUser, string token)>(
                    ServiceResultType.NotFound,
                    RoleRepository.MissingRoleExceptionMessage
                );
            }

            var roleAssignmentResult = await this.userManager.AddToRoleAsync(appUser, role);
            if (!roleAssignmentResult.Succeeded)
            {
                return new ServiceResult<(AppUser appUser, string token)>(
                    ServiceResultType.InternalError,
                    CreateErrorMessage(roleAssignmentResult.Errors)
                );
            }
        }

        if (this.appSettings.IdentitySettings.Email.RequireConfirmation)
        {
            token = await this.userManager.GenerateEmailConfirmationTokenAsync(appUser);

            if (shouldConfirmImmediately)
            {
                await this.userManager.ConfirmEmailAsync(appUser, token);
            }
        }

        return new ServiceResult<(AppUser appUser, string token)>
        {
            Result = ServiceResultType.Success,
            Data = (appUser, token),
        };
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<AppUser>> ConfirmUserEmailAsync(string email, string token)
    {
        var user = await this.userManager.FindByEmailAsync(email);
        if (user is null)
        {
            return new ServiceResult<AppUser>(ServiceResultType.NotFound);
        }

        var confirmationResult = await this.userManager.ConfirmEmailAsync(user, token);
        if (!confirmationResult.Succeeded)
        {
            return new ServiceResult<AppUser>(
                ServiceResultType.InternalError,
                CreateErrorMessage(confirmationResult.Errors)
            );
        }

        return new ServiceResult<AppUser>(ServiceResultType.Success);
    }

    /// <inheritdoc/>
    public async Task<ServiceResult<AppUser>> UpdateUserAsync(AppUser appUser)
    {
        var existingUser = await this.GetUserWithChildren(x => x.Id == appUser.Id);
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
            Data = existingUser,
        };
    }

    /// <inheritdoc/>
    public async Task<ServiceResult> RemoveUserAsync(Guid id)
    {
        var existingUser = await this.GetUserWithChildren(x => x.Id == id);
        if (existingUser is null)
        {
            return new ServiceResult(ServiceResultType.NotFound, ExceptionMessageConstants.MissingUser);
        }

        this.DatabaseContext.UserRoles.RemoveRange(existingUser.UserRoles);
        this.DatabaseContext.Users.Remove(existingUser);

        return new ServiceResult(ServiceResultType.Success);
    }

    private static bool RoleExists(IEnumerable<string> roles, string role) =>
        roles.Any(x => string.Equals(x, role, StringComparison.CurrentCultureIgnoreCase));

    private static string CreateErrorMessage(IEnumerable<IdentityError> errors) =>
        string.Join(",", errors.Select(x => x.Description));

    private async Task<AppUser> GetUserWithChildren(Expression<Func<AppUser, bool>> expression)
        => await this.DatabaseContext.Users
            .AsNoTracking()
            .Include(x => x.UserRoles)
            .ThenInclude(x => x.Role)
            .FirstOrDefaultAsync(expression);
}
