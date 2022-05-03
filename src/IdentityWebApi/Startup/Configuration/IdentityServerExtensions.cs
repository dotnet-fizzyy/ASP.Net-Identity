using IdentityWebApi.Core.ApplicationSettings;
using IdentityWebApi.Core.Entities;
using IdentityWebApi.Infrastructure.Database;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityWebApi.Startup.Configuration;

public static class IdentityServerExtensions
{
    public static void RegisterIdentityServer(
        this IServiceCollection services,
        IdentitySettings identitySettings,
        string dbConnectionString
    )
    {
        services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(dbConnectionString)
        );

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

    public static async Task InitializeUserRoles(IServiceProvider serviceProvider, ICollection<string> roles)
    {
        if (IsCollectionNullOrEmpty(roles))
        {
            return;
        }

        var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new AppRole { Name = role });
            }
        }
    }

    public static async Task InitializeDefaultUsers(
        IServiceProvider serviceProvider,
        ICollection<DefaultUserSettings> defaultUsers,
        bool requireConfirmation
    )
    {
        if (IsCollectionNullOrEmpty(defaultUsers))
        {
            return;
        }

        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

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
                Email = defaultUser.Email
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
        bool requireConfirmation
    )
    {
        if (requireConfirmation && !await userManager.IsEmailConfirmedAsync(appUserAdmin))
        {
            var token = await userManager.GenerateEmailConfirmationTokenAsync(appUserAdmin);

            await userManager.ConfirmEmailAsync(appUserAdmin, token);
        }
    }
}
