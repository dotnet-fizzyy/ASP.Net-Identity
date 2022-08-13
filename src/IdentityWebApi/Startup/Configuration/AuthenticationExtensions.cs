using IdentityWebApi.ApplicationSettings;
using IdentityWebApi.Core.Enums;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Text;
using System.Threading.Tasks;

namespace IdentityWebApi.Startup.Configuration;

/// <summary>
/// Authentication and authorization configuration.
/// </summary>
internal static class AuthenticationExtensions
{
    private const string AppAuthSchemeName = "IdentityAuthenticationScheme";

    private const string CookiesAuthType = CookieAuthenticationDefaults.AuthenticationScheme;
    private const string JwtBearerAuthType = "Bearer";

    /// <summary>
    /// Registers authentication and authorization settings and services.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <param name="identitySettings">Identity Core settings configuration.</param>
    public static void RegisterAuthSettings(this IServiceCollection services, IdentitySettings identitySettings)
    {
        services
            .AddAuthentication(opt =>
            {
                opt.DefaultScheme = AppAuthSchemeName;
                opt.DefaultChallengeScheme = AppAuthSchemeName;
                opt.DefaultAuthenticateScheme = AppAuthSchemeName;
            })
            .AddCookie(CookiesAuthType, options =>
            {
                options.SlidingExpiration = identitySettings.Cookies.SlidingExpiration;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(identitySettings.Cookies.ExpirationMinutes);

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
            })
            .AddJwtBearer(JwtBearerAuthType, opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = identitySettings.Jwt.ValidateIssuer,
                    ValidIssuer = identitySettings.Jwt.ValidIssuer,

                    ValidateAudience = identitySettings.Jwt.ValidateAudience,
                    ValidAudience = identitySettings.Jwt.ValidAudience,

                    ValidateIssuerSigningKey = identitySettings.Jwt.ValidateIssuerSigningKey,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(identitySettings.Jwt.IssuerSigningKey)),
                };
            })
            .AddPolicyScheme(AppAuthSchemeName, AppAuthSchemeName, opt =>
            {
                opt.ForwardDefaultSelector = _ =>
                {
                    if (identitySettings.AuthType == AuthType.Jwt)
                    {
                        return JwtBearerAuthType;
                    }

                    return CookiesAuthType;
                };
            });
    }
}
