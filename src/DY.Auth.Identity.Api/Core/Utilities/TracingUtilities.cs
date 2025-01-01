using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace DY.Auth.Identity.Api.Core.Utilities;

/// <summary>
/// Tracing (Open Telemetry) utilities.
/// </summary>
public static class TracingUtilities
{
    /// <summary>
    /// Sets activity display name with given format: {file_name}.{member_name}.
    /// </summary>
    /// <param name="activity">The instance of <see cref="Activity"/>.</param>
    /// <param name="filePath">Given file path to set in activity display name.</param>
    /// <param name="methodName">Given member name (function name) to set in activity display name.</param>
    public static void SetCallerDisplayName(
        this Activity activity,
        [CallerFilePath] string filePath = "",
        [CallerMemberName] string methodName = "")
    {
        if (activity == null)
        {
            return;
        }

        var fileName = Path.GetFileNameWithoutExtension(filePath);

        activity.DisplayName = $"{fileName}.{methodName}";
    }
}
