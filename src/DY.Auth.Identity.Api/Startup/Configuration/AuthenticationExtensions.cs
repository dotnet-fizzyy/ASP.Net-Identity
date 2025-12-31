using DY.Auth.Identity.Api.Core.Constants;
using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Presentation.Services;
using DY.Auth.Identity.Api.Startup.ApplicationSettings;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Threading.Tasks;

namespace DY.Auth.Identity.Api.Startup.Configuration;

/// <summary>
/// Authentication and authorization configuration.
/// </summary>
internal static class AuthenticationExtensions
{
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
                opt.DefaultScheme = AuthConstants.AppAuthPolicyName;
                opt.DefaultChallengeScheme = AuthConstants.AppAuthPolicyName;
                opt.DefaultAuthenticateScheme = AuthConstants.AppAuthPolicyName;
            })
            .AddCookie(AuthConstants.CookiesAuthScheme, options =>
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
            .AddJwtBearer(AuthConstants.JwtBearerAuthType, opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = identitySettings.Jwt.ValidateIssuer,
                    ValidIssuer = identitySettings.Jwt.ValidIssuer,

                    ValidateAudience = identitySettings.Jwt.ValidateAudience,
                    ValidAudience = identitySettings.Jwt.ValidAudience,

                    ValidateLifetime = identitySettings.Jwt.ValidateLifeTime,

                    ValidateIssuerSigningKey = identitySettings.Jwt.ValidateIssuerSigningKey,
                    IssuerSigningKey = JwtService.CreateSecuritySigningKey(identitySettings.Jwt.IssuerSigningKey),
                };
            })
            .AddPolicyScheme(AuthConstants.AppAuthPolicyName, AuthConstants.AppAuthPolicyName, opt =>
            {
                opt.ForwardDefaultSelector = _ =>
                {
                    return identitySettings.AuthType switch
                    {
                        AuthType.Jwt =>
                            AuthConstants.JwtBearerAuthType,
                        AuthType.Cookies =>
                            AuthConstants.CookiesAuthScheme,
                        _ =>
                            throw new ArgumentException("Authentication type is not provided"),
                    };
                };
            });
    }
}
