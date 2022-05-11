using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace IdentityWebApi;

/// <summary>
/// Entrypoint application class.
/// </summary>
public static class Program
{
    /// <summary>
    /// Entrypoint application method.
    /// </summary>
    /// <param name="args">Console arguments.</param>
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    private static IWebHostBuilder CreateHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, config) =>
            {
                config.AddEnvironmentVariables();
            })
            .UseStartup<Startup.Startup>();
}
