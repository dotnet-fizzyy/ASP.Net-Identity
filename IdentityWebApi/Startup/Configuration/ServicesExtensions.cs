using IdentityWebApi.BL.Interfaces;
using IdentityWebApi.BL.Services;
using IdentityWebApi.DAL.Interfaces;
using IdentityWebApi.DAL.Repository;
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
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IEmailService, EmailService>();

            //Repository
            services.AddTransient<IUserRepository, UserRepository>();
        }
    }
}