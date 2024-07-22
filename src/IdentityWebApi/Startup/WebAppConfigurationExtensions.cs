using IdentityWebApi.Startup.ApplicationSettings;
using IdentityWebApi.Startup.Configuration;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace IdentityWebApi.Startup;

// Disable UseEndpoints() non-root registration.
#pragma warning disable ASP0014

/// <summary>
/// Request pipeline configuration layer (<c>Configure</c> in past).
/// </summary>
public static class WebAppConfigurationExtensions
{
    /// <summary>
    /// Configures application HTTP request middleware.
    /// </summary>
    /// <param name="app">Instance of <see cref="WebApplication"/>.</param>
    /// <param name="appSettings">Application settings from JSON file.</param>
    public static void Configure(this WebApplication app, AppSettings appSettings)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        if (appSettings.ApiSettings.EnableSwagger)
        {
            app.UseSwaggerApp();
        }

        app.UseCors(policyBuilder => policyBuilder
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

            endpoints.RegisterHealthCheckEndpoint(appSettings.ApiSettings.EnableHealthCheckUi);
        });
    }
}
