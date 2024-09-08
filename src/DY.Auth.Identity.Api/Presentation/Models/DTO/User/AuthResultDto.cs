namespace DY.Auth.Identity.Api.Presentation.Models.DTO.User;

/// <summary>
/// Success authentication user result model.
/// </summary>
public class AuthResultDto
{
    /// <summary>
    /// Gets or sets application. access token.
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// Gets or sets user data model.
    /// </summary>
    public UserDto User { get; set; }
}
