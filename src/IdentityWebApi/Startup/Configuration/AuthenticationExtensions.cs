using IdentityWebApi.Startup.Settings;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.Startup.Configuration;

public static class AuthenticationExtensions
{
    public static void RegisterAuthSettings(this IServiceCollection services, CookiesSettings cookiesSettings)
    {
        services
            .AddAuthentication(opt =>
            {
                // Default schemes that must be applied for cookies validation
                opt.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                opt.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.SlidingExpiration = cookiesSettings.SlidingExpiration;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(cookiesSettings.ExpirationMinutes);

                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                    return Task.CompletedTask;
                };

                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;

                    return Task.CompletedTask;
                };
            });
    }
}
