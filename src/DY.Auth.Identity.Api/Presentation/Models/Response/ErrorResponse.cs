namespace DY.Auth.Identity.Api.Presentation.Models.Response;

#pragma warning disable SA1313

/// <summary>
/// Error response model.
/// </summary>
/// <param name="Message">Error message.</param>
internal record ErrorResponse(string Message)
{
    /// <summary>
    /// Gets qwe.
    /// </summary>
    public static string Code => "API_server_error";

    /// <summary>
    /// Gets or sets error source code occurence.
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    /// Gets or sets error message.
    /// </summary>
    public string Message { get; set; } = Message;

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
