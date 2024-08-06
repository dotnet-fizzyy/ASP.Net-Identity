using System.Diagnostics;

namespace DY.Auth.Identity.Api.Core.Interfaces.Infrastructure;

/// <summary>
/// Telemetry configuration.
/// </summary>
public interface IActivityTracing
{
    /// <summary>
    /// Gets an instance of <see cref="ActivitySource" />.
    /// </summary>
    ActivitySource ActivitySource { get; }
}
