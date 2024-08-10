namespace DY.Auth.Identity.Api.Presentation.Models.Response;

/// <summary>
/// Error response model.
/// </summary>
internal record ErrorResponse
{
    /// <summary>
    /// Gets or sets error source code occurence.
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// Gets or sets error message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
    /// </summary>
    /// <param name="message">Error message.</param>
    public ErrorResponse(string message)
    {
        this.Message = message;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ErrorResponse"/> class.
    /// </summary>
    /// <param name="source">Error source code occurence.</param>
    /// <param name="message">Error message.</param>
    public ErrorResponse(string source, string message)
        : this(message)
    {
        this.Source = source;
    }
}
