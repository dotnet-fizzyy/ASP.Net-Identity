using IdentityWebApi.Startup.ApplicationSettings;

using Microsoft.Extensions.DependencyInjection;

using OpenTelemetry.Exporter;
using OpenTelemetry.Instrumentation.EntityFrameworkCore;
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
                    .AddEntityFrameworkCoreInstrumentation(ConfigureEntityFramework)
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

    private static void ConfigureEntityFramework(EntityFrameworkInstrumentationOptions options)
    {
        options.SetDbStatementForText = true;
        options.SetDbStatementForStoredProcedure = true;

        options.EnrichWithIDbCommand = (activity, command) =>
        {
            activity.DisplayName = command.Connection?.Database ?? string.Empty;

            activity.SetTag("db.source", command.Connection?.ConnectionString);
            activity.SetTag("db.name", command.Connection?.Database);
            activity.SetTag("db.command.type", command.CommandType);
            activity.SetTag("db.timeout", command.CommandTimeout);
        };

        options.Filter = (_, dbCommand) =>
        {
            const string pollingSqlQuery = "SELECT 1";
            const string efMigrationSqlQuery = "__EFMigrationsHistory";

            var isPollingQuery = string.Equals(dbCommand.CommandText, pollingSqlQuery, StringComparison.OrdinalIgnoreCase);
            var isMigrationHistoryQuery = dbCommand.CommandText.Contains(efMigrationSqlQuery);

            return !isPollingQuery && !isMigrationHistoryQuery;
        };
    }

    private static void ConfigureConsoleExporter(ConsoleExporterOptions options) =>
        options.Targets = ConsoleExporterOutputTargets.Debug;

    private static void ConfigureOtlpExplorer(OtlpExporterOptions options, TelemetrySettings telemetrySettings) =>
        options.Endpoint = new Uri(telemetrySettings.OtlpExplorerEndpoint);
}
