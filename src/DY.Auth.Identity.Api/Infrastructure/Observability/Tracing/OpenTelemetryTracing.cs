using DY.Auth.Identity.Api.Core.Constants;
using DY.Auth.Identity.Api.Core.Interfaces.Infrastructure;

using System;
using System.Diagnostics;

namespace DY.Auth.Identity.Api.Infrastructure.Observability.Tracing;

/// <inheritdoc />
public class OpenTelemetryTracing : IActivityTracing
{
    /// <inheritdoc />
    public ActivitySource ActivitySource => new (AppName);

    /// <summary>
    /// Gets application name for telemetry.
    /// </summary>
    private static string AppName =>
        Environment.GetEnvironmentVariable(EnvironmentVariablesConstants.AppNameKey) ?? nameof(Api);
}
