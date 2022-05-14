using IdentityWebApi.ApplicationLogic.Validation;

namespace IdentityWebApi.ApplicationLogic.Models.Action;

/// <summary>
/// User DTO model.
/// </summary>
public class UserDto : BaseUserDto
{
    /// <summary>
    /// Gets or sets password.
    /// </summary>
    [DefaultValue]
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets user role.
    /// </summary>
    [DefaultValue]
    public string UserRole { get; set; }
}
