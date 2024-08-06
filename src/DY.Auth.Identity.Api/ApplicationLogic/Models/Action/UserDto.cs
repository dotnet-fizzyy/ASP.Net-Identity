using DY.Auth.Identity.Api.ApplicationLogic.Models.Common;

namespace DY.Auth.Identity.Api.ApplicationLogic.Models.Action;

/// <summary>
/// User DTO model.
/// </summary>
public class UserDto : BaseUser
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
