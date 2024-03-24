using IdentityWebApi.Core.Entities;
using IdentityWebApi.Core.Utilities;
using IdentityWebApi.Infrastructure.Database;
using IdentityWebApi.Infrastructure.Database.Interceptors;
using IdentityWebApi.Startup.ApplicationSettings;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityWebApi.Startup.Configuration;

/// <summary>
/// Identity Server configuration.
/// </summary>
internal static class IdentityServerExtensions
{
    /// <summary>
    /// Configures Identity Server service.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <param name="identitySettings"><see cref="IdentitySettings"/>.</param>
    /// <param name="dbSettings">The instance of <see cref="dbSettings"/>.</param>
    public static void RegisterIdentityServer(
        this IServiceCollection services,
        IdentitySettings identitySettings,
        DbSettings dbSettings)
    {
        services.AddDbContext<DatabaseContext>(options =>
            options
                .UseSqlServer(dbSettings.ConnectionString)
                .AddInterceptors(
                    new ExecutionThresholdExceedSqlInterceptor(
                        logger: services.BuildServiceProvider().GetRequiredService<ILogger<ExecutionThresholdExceedSqlInterceptor>>(),
                        sqlQueryExecutionThresholdInMilliseconds: dbSettings.SqlQueryExecutionThresholdInMilliseconds)));

        services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = identitySettings.Email.RequiredUniqueEmail;
                options.Password.RequireDigit = identitySettings.Password.RequireDigit;
                options.Password.RequireLowercase = identitySettings.Password.RequireLowercase;
                options.Password.RequireUppercase = identitySettings.Password.RequireUppercase;
                options.Password.RequireNonAlphanumeric = identitySettings.Password.RequireNonAlphanumeric;
                options.Password.RequiredLength = identitySettings.Password.RequiredLength;
                options.Password.RequiredUniqueChars = identitySettings.Password.RequiredUniqueChars;
            })
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddDefaultTokenProviders();
    }

    /// <summary>
    /// Creates default user roles on application start.
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/>.</param>
    /// <param name="roles">Collection of role names to create.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task InitializeUserRoles(this IServiceProvider serviceProvider, ICollection<string> roles)
    {
        using var scope = serviceProvider.CreateScope();

        if (roles.IsNullOrEmpty())
        {
            return;
        }

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        foreach (var role in roles)
        {
            var isRoleAlreadyExists = await roleManager.RoleExistsAsync(role);

            if (isRoleAlreadyExists)
            {
                continue;
            }

            await roleManager.CreateAsync(new AppRole { Name = role });
        }
    }

    /// <summary>
    /// Creates default users on application start.
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/>.</param>
    /// <param name="defaultUsers">Collection of users to create.</param>
    /// <param name="requireConfirmation">Whether confirm immediately user emails.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public static async Task InitializeDefaultUsers(
        this IServiceProvider serviceProvider,
        ICollection<DefaultUserSettings> defaultUsers,
        bool requireConfirmation)
    {
        using var scope = serviceProvider.CreateScope();

        if (defaultUsers.IsNullOrEmpty())
        {
            return;
        }

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        foreach (var defaultUser in defaultUsers)
        {
            var isRequiredUserInformationMissing = string.IsNullOrEmpty(defaultUser.Name) ||
                                                   string.IsNullOrEmpty(defaultUser.Password) ||
                                                   string.IsNullOrEmpty(defaultUser.Role) ||
                                                   string.IsNullOrEmpty(defaultUser.Email);

            if (isRequiredUserInformationMissing)
            {
                return;
            }

            var existingAdmin = await userManager.FindByEmailAsync(defaultUser.Email);

            if (existingAdmin != null)
            {
                await ConfirmDefaultAdminEmail(userManager, existingAdmin, requireConfirmation);

                return;
            }

            var appUserAdmin = new AppUser
            {
                UserName = defaultUser.Name,
                Email = defaultUser.Email,
            };

            await userManager
                .CreateAsync(appUserAdmin, defaultUser.Password)
                .ContinueWith(
                    _ => userManager.AddToRoleAsync(appUserAdmin, defaultUser.Role),
                    TaskContinuationOptions.NotOnFaulted | TaskContinuationOptions.NotOnCanceled)
                .ContinueWith(
                    _ => ConfirmDefaultAdminEmail(userManager, appUserAdmin, requireConfirmation),
                    TaskContinuationOptions.NotOnFaulted | TaskContinuationOptions.NotOnCanceled);
        }
    }

    private static async Task ConfirmDefaultAdminEmail(
        UserManager<AppUser> userManager,
        AppUser appUserAdmin,
        bool requireConfirmation)
    {
        var isEmailAlreadyConfirmed = await userManager.IsEmailConfirmedAsync(appUserAdmin);

        if (requireConfirmation && !isEmailAlreadyConfirmed)
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(appUserAdmin);

            await userManager.ConfirmEmailAsync(appUserAdmin, token);
        }
    }
}
