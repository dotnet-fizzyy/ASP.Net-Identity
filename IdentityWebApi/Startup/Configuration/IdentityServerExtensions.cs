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
        public static void RegisterIdentityServer(this IServiceCollection services, DbSettings dbSettings)
        {
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(dbSettings.ConnectionString));
            
            services.AddIdentity<User, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();
        }

        public static async Task InitializeUserRoles(IServiceProvider serviceProvider, ICollection<string> roles)
        {
            if (roles == null || !roles.Any())
            {
                return;
            }
            
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid> { Name = role });
                }
            }
        }
    }
}