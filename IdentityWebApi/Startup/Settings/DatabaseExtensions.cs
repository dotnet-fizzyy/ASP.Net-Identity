using IdentityWebApi.DAL;
using IdentityWebApi.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityWebApi.Startup.Settings
{
    public static class DatabaseExtensions
    {
        public static void RegisterDatabase(this IServiceCollection services, DbSettings dbSettings)
        {
            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(dbSettings.ConnectionString));
            
            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>();
        }
    }
}