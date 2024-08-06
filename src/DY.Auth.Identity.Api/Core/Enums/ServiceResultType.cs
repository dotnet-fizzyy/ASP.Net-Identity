namespace DY.Auth.Identity.Api.Core.Enums;

/// <summary>
/// Operation service result statuses.
/// </summary>
public enum ServiceResultType
{
    /// <summary>
    /// Gets success result.
    /// </summary>
    Success = 1,

    /// <summary>
    /// Gets unauthenticated (undefined identity) result.
    /// </summary>
    Unauthenticated,

    /// <summary>
    /// Gets unauthorized (lack of permissions) result.
    /// </summary>
    Unauthorized,

    /// <summary>
    /// Gets invalid data result.
    /// </summary>
    InvalidData,

    /// <summary>
    /// Gets not found result.
    /// </summary>
    NotFound,

    /// <summary>
    /// Gets internal error result.
    /// </summary>
    InternalError,
}
