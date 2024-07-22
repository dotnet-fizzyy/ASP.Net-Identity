using IdentityWebApi.Core.Constants;
using IdentityWebApi.Core.Interfaces.Infrastructure;

using System;
using System.Diagnostics;

namespace IdentityWebApi.Infrastructure.Observability.Tracing;

/// <inheritdoc />
public class OpenTelemetryTracing : IActivityTracing
{
    /// <inheritdoc />
    public ActivitySource ActivitySource => new ActivitySource(AppName);

    /// <summary>
    /// Gets application name for telemetry.
    /// </summary>
    private static string AppName =>
        Environment.GetEnvironmentVariable(EnvironmentVariablesConstants.AppNameKey) ?? nameof(IdentityWebApi);
}
