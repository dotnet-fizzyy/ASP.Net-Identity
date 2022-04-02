using IdentityWebApi.BL.Interfaces;
using IdentityWebApi.BL.Services;
using IdentityWebApi.DAL;
using IdentityWebApi.DAL.Interfaces;
using IdentityWebApi.DAL.Repository;
using IdentityWebApi.PL.Interfaces;
using IdentityWebApi.PL.Services;
using IdentityWebApi.Startup.Settings;

using Microsoft.Extensions.DependencyInjection;

namespace IdentityWebApi.Startup.Configuration;

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
        services.AddTransient<IClaimsService, ClaimsService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IEmailTemplateService, EmailTemplateService>();

        services.AddScoped<IHttpContextService, HttpContextService>();

        //Repository
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
    }
}
