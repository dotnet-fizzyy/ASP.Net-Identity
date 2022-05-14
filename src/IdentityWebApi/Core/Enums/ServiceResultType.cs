namespace IdentityWebApi.Core.Enums;

/// <summary>
/// ServiceResult status codes.
/// </summary>
public enum ServiceResultType
{
    /// <summary>
    /// Gets OK HTTP response status.
    /// </summary>
    Success = 200,

    /// <summary>
    /// Gets Bad Request HTTP response status.
    /// </summary>
    InvalidData = 400,

    /// <summary>
    /// Gets Not Found HTTP response status.
    /// </summary>
    NotFound = 404,

    /// <summary>
    /// Gets Internal Server Error HTTP response status.
    /// </summary>
    InternalError = 500,
}
