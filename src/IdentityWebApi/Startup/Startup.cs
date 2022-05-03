using IdentityWebApi.Core.ApplicationSettings;
using IdentityWebApi.Startup.Configuration;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;

namespace IdentityWebApi.Startup;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        this.Configuration = configuration;
    }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        var appSettings = ReadAppSettings(this.Configuration);

        services.ValidateSettingParameters(this.Configuration);

        services.RegisterServices(appSettings);

        // Identity server setup should go before Auth setup
        services.RegisterIdentityServer(appSettings.IdentitySettings, appSettings.DbSettings.ConnectionString);
        services.RegisterAuthSettings(appSettings.IdentitySettings.Cookies);

        services.RegisterAutomapper();

        services.RegisterHealthChecks(appSettings.DbSettings.ConnectionString);

        services.AddHttpContextAccessor();

        services.AddRouting(opts =>
        {
            opts.LowercaseUrls = true;
        });

        services.RegisterControllers();

        services.RegisterSwagger();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
    {
        var appSettings = ReadAppSettings(this.Configuration);

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwaggerApp();
        }

        app.UseCors(x => x
            .SetIsOriginAllowed(_ => true)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
        );

        app.UseStaticFiles();

        app.RegisterExceptionHandler();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();

            endpoints.RegisterHealthCheckEndpoint();
        });

        IdentityServerExtensions.InitializeUserRoles(serviceProvider, appSettings.IdentitySettings.Roles).Wait();
        IdentityServerExtensions.InitializeDefaultUsers(
            serviceProvider,
            appSettings.IdentitySettings.DefaultUsers,
            appSettings.IdentitySettings.Email.RequireConfirmation
        ).Wait();
    }

    private static AppSettings ReadAppSettings(IConfiguration configuration)
    {
        var dbSettings = configuration.GetSection(nameof(AppSettings.DbSettings)).Get<DbSettings>();
        var smtpClientSettings = configuration.GetSection(nameof(AppSettings.SmtpClientSettings)).Get<SmtpClientSettings>();
        var ipStackSettings = configuration.GetSection(nameof(AppSettings.IpStackSettings)).Get<IpStackSettings>();
        var regionVerification = configuration.GetSection(nameof(AppSettings.RegionsVerificationSettings)).Get<RegionsVerificationSettings>();
        var identitySettings = configuration.GetSection(nameof(AppSettings.IdentitySettings)).Get<IdentitySettings>();

        return new AppSettings
        {
            DbSettings = dbSettings,
            SmtpClientSettings = smtpClientSettings,
            IpStackSettings = ipStackSettings,
            RegionsVerificationSettings = regionVerification,
            IdentitySettings = identitySettings,
        };
    }
}