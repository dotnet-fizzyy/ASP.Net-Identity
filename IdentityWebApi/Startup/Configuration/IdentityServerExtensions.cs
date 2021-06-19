using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityWebApi.DAL;
using IdentityWebApi.DAL.Entities;
using IdentityWebApi.Startup.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityWebApi.Startup.Configuration
{
    public static class IdentityServerExtensions
    {
        public static void RegisterIdentityServer(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(appSettings.DbSettings.ConnectionString));
            
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
            if (roles == null || !roles.Any())
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

        public static async Task InitializeDefaultAdmin(IServiceProvider serviceProvider, DefaultAdminSettings defaultAdmin)
        {
            if (defaultAdmin is null || 
                string.IsNullOrEmpty(defaultAdmin.Name) ||
                string.IsNullOrEmpty(defaultAdmin.Password) || 
                string.IsNullOrEmpty(defaultAdmin.Role) || 
                string.IsNullOrEmpty(defaultAdmin.Email))
            {
                return;
            }

            var userManage = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            var existingAdmin = await userManage.FindByEmailAsync(defaultAdmin.Email);
            if (existingAdmin is not null)
            {
                return;
            }
            
            var appUserAdmin = new AppUser
            {
                UserName = defaultAdmin.Name,
                Email = defaultAdmin.Email
            };
            
            await userManage.CreateAsync(appUserAdmin, defaultAdmin.Password);
            await userManage.AddToRoleAsync(appUserAdmin, defaultAdmin.Role);
        }
    }
}