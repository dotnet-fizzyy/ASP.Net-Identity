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
    public static void RegisterIdentityServer(this IServiceCollection services, AppSettings appSettings)
    {
        services.AddDbContext<DatabaseContext>(options =>
            options.UseSqlServer(appSettings.DbSettings.ConnectionString));

        services.AddIdentity<AppUser, AppRole>(options =>
            {
                options.User.RequireUniqueEmail = appSettings.IdentitySettings.Email.RequiredUniqueEmail;
                options.Password.RequireDigit = appSettings.IdentitySettings.Password.RequireDigit;
                options.Password.RequireLowercase = appSettings.IdentitySettings.Password.RequireLowercase;
                options.Password.RequireUppercase = appSettings.IdentitySettings.Password.RequireUppercase;
                options.Password.RequireNonAlphanumeric = appSettings.IdentitySettings.Password.RequireNonAlphanumeric;
                options.Password.RequiredLength = appSettings.IdentitySettings.Password.RequiredLength;
                options.Password.RequiredUniqueChars = appSettings.IdentitySettings.Password.RequiredUniqueChars;
            })
            .AddRoles<AppRole>()
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddDefaultTokenProviders();
    }

    public static async Task InitializeUserRoles(IServiceProvider serviceProvider, ICollection<string> roles)
    {
        if (IsCollectionEmpty(roles))
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
        if (IsCollectionEmpty(defaultUsers))
        {
            return;
        }

        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        foreach (var defaultUser in defaultUsers)
        {
            if (string.IsNullOrEmpty(defaultUser.Name) ||
                string.IsNullOrEmpty(defaultUser.Password) ||
                string.IsNullOrEmpty(defaultUser.Role) ||
                string.IsNullOrEmpty(defaultUser.Email))
            {
                return;
            }

            var existingAdmin = await userManager.FindByEmailAsync(defaultUser.Email);
            if (existingAdmin is not null)
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


    private static bool IsCollectionEmpty<T>(ICollection<T> collection) => collection is null || !collection.Any(); 
    
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
