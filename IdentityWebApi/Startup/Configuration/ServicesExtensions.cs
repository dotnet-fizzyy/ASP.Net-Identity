using IdentityWebApi.BL.Interfaces;
using IdentityWebApi.BL.Services;
using IdentityWebApi.Startup.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityWebApi.Startup.Configuration
{
    public static class ServicesExtensions
    {
        public static void RegisterServices(this IServiceCollection services, AppSettings appSettings)
        {
            //AppSettings
            services.AddSingleton(appSettings);
            
            //Services
            services.AddTransient<IUserService, UserService>();
        }
    }
}