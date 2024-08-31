namespace DY.Auth.Identity.Api.Presentation.Models.DTO.User;

/// <summary>
/// Success authentication user result model.
/// </summary>
public class AuthResultDto
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
