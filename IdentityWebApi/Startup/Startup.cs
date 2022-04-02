using HealthChecks.UI.Client;

using IdentityWebApi.Startup.Configuration;
using IdentityWebApi.Startup.Settings;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

using System;
using System.Text.Json.Serialization;

namespace IdentityWebApi.Startup
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettings = ReadAppSettings(Configuration);

            services.ValidateSettingParameters(Configuration);
            
            services.RegisterServices(appSettings);

            services.RegisterIdentityServer(appSettings); // Identity server setup should go before Auth setup
            services.RegisterAuthSettings(appSettings.IdentitySettings.Cookies);
            
            services.RegisterAutomapper();
            
            services.RegisterHealthChecks(appSettings.DbSettings.ConnectionString);
            
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault;
            });
            
            services.RegisterSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            var appSettings = ReadAppSettings(Configuration);
            
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

                endpoints.MapHealthChecks("/health-check", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
                endpoints.MapHealthChecksUI(options =>
                {
                    options.UIPath = "/health-check-ui";
                });
            });

            IdentityServerExtensions.InitializeUserRoles(serviceProvider, appSettings.IdentitySettings.Roles).Wait();
            IdentityServerExtensions.InitializeDefaultUser(serviceProvider, appSettings.IdentitySettings.DefaultUsers, appSettings.IdentitySettings.Email.RequireConfirmation).Wait();
        }

        private static AppSettings ReadAppSettings(IConfiguration configuration)
        {
            var dbSettings = configuration.GetSection(nameof(AppSettings.DbSettings)).Get<DbSettings>();
            var smtpClientSettings = configuration.GetSection(nameof(AppSettings.SmtpClientSettings)).Get<SmtpClientSettings>();
            var identitySettings = configuration.GetSection(nameof(AppSettings.IdentitySettings)).Get<IdentitySettings>();

            return new AppSettings
            {
                DbSettings = dbSettings,
                SmtpClientSettings = smtpClientSettings,
                IdentitySettings = identitySettings
            };
        }
    }
}