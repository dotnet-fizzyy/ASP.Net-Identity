using IdentityWebApi.Startup.ApplicationSettings;
using IdentityWebApi.Startup.Configuration;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;

namespace IdentityWebApi.Startup;

/// <summary>
/// Configuration of middleware and services.
/// </summary>
public class Startup
{
    private readonly AppSettings appSettings;

    /// <inheritdoc cref="IConfiguration"/>
    public IConfiguration Configuration { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Startup"/> class.
    /// </summary>
    /// <param name="configuration"><see cref="IConfiguration"/>.</param>
    public Startup(IConfiguration configuration)
    {
        this.Configuration = configuration;

        this.appSettings = ReadAppSettings(configuration);
    }

    /// <summary>
    /// Configures used services in whole application.
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/>.</param>
    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.ValidateSettingParameters(this.Configuration);

        services.RegisterServices(this.appSettings);

        // Identity server setup should go before Auth setup
        services.RegisterIdentityServer(this.appSettings.IdentitySettings, this.appSettings.DbSettings.ConnectionString);
        services.RegisterAuthSettings(this.appSettings.IdentitySettings);

        services.RegisterMediatr();
        services.RegisterValidationPipeline();

        services.RegisterAutomapper();

        services.RegisterHealthChecks(this.appSettings.Api.Url, this.appSettings.DbSettings.ConnectionString);

        services.AddHttpContextAccessor();

        services.AddRouting(opts =>
        {
            opts.LowercaseUrls = true;
        });

        services.RegisterControllers();

        services.RegisterSwagger(this.appSettings.IdentitySettings);
    }

    /// <summary>
    /// Configures application HTTP middleware.
    /// </summary>
    /// <param name="app"><see cref="IApplicationBuilder"/>.</param>
    /// <param name="env"><see cref="IWebHostEnvironment"/>.</param>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/>.</param>
    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
    {
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

        serviceProvider.InitializeUserRoles(this.appSettings.IdentitySettings.Roles).Wait();
        serviceProvider.InitializeDefaultUsers(
            this. appSettings.IdentitySettings.DefaultUsers,
            this.appSettings.IdentitySettings.Email.RequireConfirmation
        ).Wait();
    }

    private static AppSettings ReadAppSettings(IConfiguration configuration)
    {
        var apiSettings = configuration
            .GetSection(nameof(AppSettings.Api))
            .Get<ApiSettings>();
        var dbSettings = configuration
            .GetSection(nameof(AppSettings.DbSettings))
            .Get<DbSettings>();
        var smtpClientSettings = configuration
            .GetSection(nameof(AppSettings.SmtpClientSettings))
            .Get<SmtpClientSettings>();
        var ipStackSettings = configuration
            .GetSection(nameof(AppSettings.IpStackSettings))
            .Get<IpStackSettings>();
        var regionVerification = configuration
            .GetSection(nameof(AppSettings.RegionsVerificationSettings))
            .Get<RegionsVerificationSettings>();
        var identitySettings = configuration
            .GetSection(nameof(AppSettings.IdentitySettings))
            .Get<IdentitySettings>();

        return new AppSettings
        {
            Api = apiSettings,
            DbSettings = dbSettings,
            SmtpClientSettings = smtpClientSettings,
            IpStackSettings = ipStackSettings,
            RegionsVerificationSettings = regionVerification,
            IdentitySettings = identitySettings,
        };
    }
}