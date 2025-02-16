using DY.Auth.Identity.Api.Core.Constants;
using DY.Auth.Identity.Api.Core.Interfaces.Infrastructure;

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace DY.Auth.Identity.Api.Infrastructure.Observability.Tracing;

/// <inheritdoc />
public class OpenTelemetryTracing : IActivityTracing
{
    /// <inheritdoc />
    public Activity StartActivity([CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "")
    {
        using var activitySource = new ActivitySource(AppName);

        using var activity = activitySource.StartActivity();

        if (activity != null)
        {
            activity.DisplayName = GetActivityDisplayName(filePath, memberName);
        }

        return activity;
    }

    /// <summary>
    /// Gets application name for telemetry.
    /// </summary>
    private static string AppName =>
        Environment.GetEnvironmentVariable(EnvironmentVariablesConstants.AppNameKey) ?? nameof(Api);

    private static string GetActivityDisplayName(string filePath, string memberName)
    {
        var fileName = Path.GetFileNameWithoutExtension(filePath);

        return $"{fileName}.{memberName}";
    }
}
