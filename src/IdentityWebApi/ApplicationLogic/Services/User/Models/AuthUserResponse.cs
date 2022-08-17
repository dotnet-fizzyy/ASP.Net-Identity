using IdentityWebApi.ApplicationLogic.Models.Action;

namespace IdentityWebApi.ApplicationLogic.Services.User.Models;

/// <summary>
/// Success authentication user response.
/// </summary>
public class AuthUserResponse
{
    /// <summary>
    /// Gets or sets JWT.
    /// </summary>
    public string Jwt { get; set; }

    /// <summary>
    /// Gets or sets user data model.
    /// </summary>
    public UserResultDto User { get; set; }
}