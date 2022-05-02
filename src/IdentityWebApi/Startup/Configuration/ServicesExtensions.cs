using IdentityWebApi.ApplicationLogic.Services;
using IdentityWebApi.Core.ApplicationSettings;
using IdentityWebApi.Core.Interfaces.ApplicationLogic;
using IdentityWebApi.Core.Interfaces.Infrastructure;
using IdentityWebApi.Core.Interfaces.Presentation;
using IdentityWebApi.Infrastructure;
using IdentityWebApi.Infrastructure.Net.Services;
using IdentityWebApi.Infrastructure.Repository;
using IdentityWebApi.Presentation.Services;

using Microsoft.Extensions.DependencyInjection;

namespace IdentityWebApi.Startup.Configuration;

public static class ServicesExtensions
{
    public static void RegisterServices(this IServiceCollection services, AppSettings appSettings)
    {
        // AppSettings
        services.AddSingleton(appSettings);

        // Services
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<IAuthService, AuthService>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IClaimsService, ClaimsService>();
        services.AddTransient<IRoleService, RoleService>();
        services.AddTransient<IEmailTemplateService, EmailTemplateService>();
        services.AddSingleton<INetService, NetService>();

        services.AddScoped<IHttpContextService, HttpContextService>();

        // Repository
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>();
    }
}
