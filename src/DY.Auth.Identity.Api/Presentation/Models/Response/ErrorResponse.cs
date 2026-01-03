using System;

namespace DY.Auth.Identity.Api.Presentation.Models.Response;

/// <summary>
/// Error response model.
/// </summary>
internal record ErrorResponse
{
    /// <summary>
    /// Gets unique error ID.
    /// </summary>
    public Guid ErrorId { get; } = Guid.NewGuid();

    /// <summary>
    /// Gets or sets error code.
    /// </summary>
    public string ErrorCode { get; set; }

    /// <summary>
    /// Gets or sets HTTP response status code.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Gets or sets error source code occurence.
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// Gets or sets error message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets UNIX timestamp when instances gets created.
    /// </summary>
    public long Timestamp { get; } = GetTimestamp();

    private static long GetTimestamp()
    {
        return ((DateTimeOffset)DateTime.UtcNow).ToUnixTimeSeconds();
    }
}
