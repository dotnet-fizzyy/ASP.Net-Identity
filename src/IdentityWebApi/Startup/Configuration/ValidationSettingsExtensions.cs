using IdentityWebApi.Core.ApplicationSettings;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityWebApi.Startup.Configuration;

public static class ValidationSettingsExtensions
{
    public static void ValidateSettingParameters(this IServiceCollection services, IConfiguration configuration)
    {
        services.UseConfigurationValidation();
        services.ConfigureValidatableSetting<AppSettings>(configuration);
    }
}
