namespace DY.Auth.Identity.Api.Presentation.Models.DTO.User;

/// <summary>
/// User registration DTO model.
/// </summary>
public class UserRegistrationDto
{
    /// <summary>
    /// Gets or sets email.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets password.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets username.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets role.
    /// </summary>
    public string Role { get; set; }
}
