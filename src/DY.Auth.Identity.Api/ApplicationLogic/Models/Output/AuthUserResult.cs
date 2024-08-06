namespace DY.Auth.Identity.Api.ApplicationLogic.Models.Output;

/// <summary>
/// Success authentication user result.
/// </summary>
public class AuthUserResult
{
    /// <summary>
    /// Gets or sets token.
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// Gets or sets user data model.
    /// </summary>
    public UserResult User { get; set; }
}
