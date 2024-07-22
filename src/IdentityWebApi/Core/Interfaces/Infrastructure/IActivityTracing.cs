using System.Diagnostics;

namespace IdentityWebApi.Core.Interfaces.Infrastructure;

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
