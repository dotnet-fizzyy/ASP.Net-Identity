using IdentityWebApi.Startup.ApplicationSettings;
using IdentityWebApi.Startup.Configuration;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace IdentityWebApi.Startup;

/// <summary>
/// Request pipeline configuration layer (<c>Configure</c> in past).
/// </summary>
public static class WebAppConfigurationExtensions
{
    /// <summary>
    /// Configures application HTTP request middleware.
    /// </summary>
    /// <param name="app">
    /// Instance of <see cref="WebApplication"/>.
    /// </param>
    /// <param name="appSettings">
    /// Application settings from "appsettings.json" file.
    /// </param>
    public static void Configure(this WebApplication app, AppSettings appSettings)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwaggerApp();
        }

        app.UseCors(x => x
            .SetIsOriginAllowed(_ => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.RegisterExceptionHandler();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.RegisterProxyServerHeaders();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            endpoints.RegisterHealthCheckEndpoint();
        });

        app.Services
            .InitializeUserRoles(appSettings.IdentitySettings.Roles)
            .Wait();
        app.Services
            .InitializeDefaultUsers(
                appSettings.IdentitySettings.DefaultUsers,
                appSettings.IdentitySettings.Email.RequireConfirmation)
            .Wait();
    }
}