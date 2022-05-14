using System.Diagnostics.CodeAnalysis;

namespace IdentityWebApi.Presentation.Models.Response;

/// <summary>
/// Error response model.
/// </summary>
/// <param name="Message">qwe.</param>
[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1313:Parameter names should begin with lower-case letter", Justification = "<Pending />")]
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