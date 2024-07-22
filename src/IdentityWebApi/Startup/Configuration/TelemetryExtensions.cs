using IdentityWebApi.Startup.ApplicationSettings;

using Microsoft.Extensions.DependencyInjection;

using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

using System;

namespace IdentityWebApi.Startup.Configuration;

/// <summary>
/// Open Telemetry configuration extensions.
/// </summary>
public static class TelemetryExtensions
{
    /// <summary>
    /// Registers configuration of Open Telemetry.
    /// </summary>
    /// <param name="services">The instance of <see cref="IServiceCollection"/>.</param>
    /// <param name="telemetrySettings">Open Telemetry configuration settings.</param>
    public static void RegisterOpenTelemetry(this IServiceCollection services, TelemetrySettings telemetrySettings)
    {
        var resourceBuilder = GetResourceBuilder(
            telemetrySettings.AppName,
            telemetrySettings.Namespace,
            telemetrySettings.Version);

        services
            .AddOpenTelemetry()
            .WithMetrics(builder =>
            {
                builder
                    .SetResourceBuilder(resourceBuilder)
                    .AddMeter(telemetrySettings.AppName)
                    .AddAspNetCoreInstrumentation()
                    .AddConsoleExporter(ConfigureConsoleExporter)
                    .AddHttpClientInstrumentation()
                    .AddProcessInstrumentation()
                    .AddRuntimeInstrumentation()
                    .AddOtlpExporter(otlpExporterOptions => ConfigureOtlpExplorer(otlpExporterOptions, telemetrySettings));
            })
            .WithTracing(builder =>
            {
                builder
                    .SetResourceBuilder(resourceBuilder)
                    .AddAspNetCoreInstrumentation(instrumentationBuilder =>
                    {
                        instrumentationBuilder.EnrichWithException = (activity, exception) =>
                            activity?.RecordException(exception);

                        instrumentationBuilder.Filter = httpContext =>
                            !httpContext.Request.Path.StartsWithSegments(
                                "/health",
                                StringComparison.CurrentCultureIgnoreCase);
                    })
                    .AddConsoleExporter(ConfigureConsoleExporter)
                    .AddHttpClientInstrumentation()
                    .AddEntityFrameworkCoreInstrumentation()
                    .AddSource(telemetrySettings.AppName)
                    .AddOtlpExporter(otlpExporterOptions => ConfigureOtlpExplorer(otlpExporterOptions, telemetrySettings));
            });
    }

    private static ResourceBuilder GetResourceBuilder(
        string serviceName,
        string serviceNamespace,
        string serviceVersion) =>
            ResourceBuilder
                .CreateDefault()
                .AddService(
                    serviceName: serviceName,
                    serviceNamespace: serviceNamespace,
                    serviceVersion: serviceVersion);

    private static void ConfigureConsoleExporter(ConsoleExporterOptions options) =>
        options.Targets = ConsoleExporterOutputTargets.Debug;

    private static void ConfigureOtlpExplorer(OtlpExporterOptions options, TelemetrySettings telemetrySettings) =>
        options.Endpoint = new Uri(telemetrySettings.OtlpExplorerEndpoint);
}
