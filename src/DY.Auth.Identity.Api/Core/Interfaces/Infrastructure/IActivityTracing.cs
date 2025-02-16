using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace DY.Auth.Identity.Api.Core.Interfaces.Infrastructure;

/// <summary>
/// Telemetry configuration.
/// </summary>
public interface IActivityTracing
{
    /// <summary>
    /// Starts <see cref="ActivitySource"/> and returns <see cref="Activity" /> with display name in format: {file_name}.{member_name}.
    /// </summary>
    /// <param name="filePath">Runtime file path to set in activity display name.</param>
    /// <param name="memberName">Runtime member name (function name) to set in activity display name.</param>
    /// <returns>The instance of <see cref="Activity"/>.</returns>
    Activity StartActivity([CallerFilePath] string filePath = "", [CallerMemberName] string memberName = "");
}
