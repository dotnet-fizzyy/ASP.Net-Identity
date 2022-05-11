using IdentityWebApi.Core.ApplicationSettings;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Threading.Tasks;

namespace IdentityWebApi.Startup.Configuration;

/// <summary>
/// Authentication and authorization configuration.
/// </summary>
internal static class AuthenticationExtensions
{
    /// <summary>
    /// Registers authentication and authorization settings and services.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <param name="cookiesSettings">Cookies settings configuration.</param>
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
