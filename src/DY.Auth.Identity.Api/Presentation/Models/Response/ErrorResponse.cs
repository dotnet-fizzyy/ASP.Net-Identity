using System;

namespace DY.Auth.Identity.Api.Presentation.Models.Response;

/// <summary>
/// Error response model.
/// </summary>
internal record ErrorResponse
{
    /// <summary>
    /// Gets or sets unique error ID.
    /// </summary>
    public Guid ErrorId { get; set; }

    /// <summary>
    /// Gets or sets error code.
    /// </summary>
    public string ErrorCode { get; set; }

    /// <summary>
    /// Gets or sets error source code occurence.
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// Gets or sets error message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets UNIX timestamp.
    /// </summary>
    public long Timestamp { get; set; }
}
