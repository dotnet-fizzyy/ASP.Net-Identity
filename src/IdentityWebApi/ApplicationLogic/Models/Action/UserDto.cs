namespace IdentityWebApi.ApplicationLogic.Models.Action;

/// <summary>
/// User DTO model.
/// </summary>
public class UserDto : BaseUserDto
{
    /// <summary>
    /// Gets or sets password.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets user role.
    /// </summary>
    public string UserRole { get; set; }
}
