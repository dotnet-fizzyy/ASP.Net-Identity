namespace IdentityWebApi.Presentation.Models.Response;

internal record ErrorResponse(string Message)
{
    public static string Code => "API_server_error";

    public string Source { get; set; }

    public string Message { get; set; } = Message;

    public ErrorResponse(string source, string message)
        : this(message)
    {
        Source = source;
    }
}