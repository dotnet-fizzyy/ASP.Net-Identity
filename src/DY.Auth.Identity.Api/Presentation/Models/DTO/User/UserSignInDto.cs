namespace DY.Auth.Identity.Api.Presentation.Models.DTO.User;

/// <summary>
/// User sign in DTO model.
/// </summary>
public class UserSignInDto
{
    /// <summary>
    /// Gets or sets email.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets password.
    /// </summary>
    public string Password { get; set; }
}
