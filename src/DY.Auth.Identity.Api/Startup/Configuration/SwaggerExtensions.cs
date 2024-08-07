using DY.Auth.Identity.Api.Core.Enums;
using DY.Auth.Identity.Api.Startup.ApplicationSettings;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using System;
using System.IO;
using System.Reflection;

namespace DY.Auth.Identity.Api.Startup.Configuration;

/// <summary>
/// Swagger configuration.
/// </summary>
internal static class SwaggerExtensions
{
    /// <summary>
    /// Configures Swagger services.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    /// <param name="identitySettings"><see cref="IdentitySettings"/>.</param>
    public static void RegisterSwagger(this IServiceCollection services, IdentitySettings identitySettings)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "DY.Auth.Identity.Api",
                Version = "v1",
                Description = "Identity Web API for managing users",
                Contact = new OpenApiContact
                {
                    Name = "Fizzyy",
                    Email = "ezzyfizzy27@gmail.com",
                },
            });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            options.IncludeXmlComments(xmlPath);

            if (identitySettings.AuthType != AuthType.Jwt)
            {
                return;
            }

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization via Bearer scheme: Bearer {token}",
                Scheme = "JWT",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer",
                        },
                    },
                    Array.Empty<string>()
                },
            });
        });
    }

    /// <summary>
    /// Configures Swagger middleware.
    /// </summary>
    /// <param name="app"><see cref="IApplicationBuilder"/>.</param>
    public static void UseSwaggerApp(this IApplicationBuilder app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DY.Auth.Identity.Api v1"));
    }
}
