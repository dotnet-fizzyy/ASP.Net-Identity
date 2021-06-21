using System;
using System.Net;
using System.Threading.Tasks;
using IdentityWebApi.Startup.Settings;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityWebApi.Startup.Configuration
{
    public static class AuthenticationExtensions
    {
        public static void RegisterAuthSettings(this IServiceCollection services, CookiesSettings cookiesSettings)
        {
            services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.SlidingExpiration = cookiesSettings.SlidingExpiration;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(cookiesSettings.ExpirationMinutes);
                    
                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        
                        return Task.CompletedTask;
                    };
                    
                    options.Events.OnRedirectToAccessDenied = context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                        
                        return Task.CompletedTask;
                    };
                });
        }
    }
}