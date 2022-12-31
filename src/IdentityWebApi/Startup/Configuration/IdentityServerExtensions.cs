using IdentityWebApi.Core.Entities;
using IdentityWebApi.Infrastructure.Database;
using IdentityWebApi.Startup.ApplicationSettings;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
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
    /// <param name="dbConnectionString">Database connection link.</param>
    public static void RegisterIdentityServer(
        this IServiceCollection services,
        IdentitySettings identitySettings,
        string dbConnectionString)
    {
        services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(dbConnectionString));

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

        if (IsCollectionNullOrEmpty(roles))
        {
            return;
        }

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new AppRole { Name = role });
            }
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

        if (IsCollectionNullOrEmpty(defaultUsers))
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

            await userManager.CreateAsync(appUserAdmin, defaultUser.Password);
            await userManager.AddToRoleAsync(appUserAdmin, defaultUser.Role);

            await ConfirmDefaultAdminEmail(userManager, appUserAdmin, requireConfirmation);
        }
    }

    private static bool IsCollectionNullOrEmpty<T>(ICollection<T> collection) =>
        collection == null || !collection.Any();

    private static async Task ConfirmDefaultAdminEmail(
        UserManager<AppUser> userManager,
        AppUser appUserAdmin,
        bool requireConfirmation)
    {
        if (requireConfirmation && !await userManager.IsEmailConfirmedAsync(appUserAdmin))
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(appUserAdmin);

            await userManager.ConfirmEmailAsync(appUserAdmin, token);
        }
    }
}
