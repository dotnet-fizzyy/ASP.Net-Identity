using IdentityWebApi.ApplicationLogic.Validation;

namespace IdentityWebApi.ApplicationLogic.Models.Action;

/// <summary>
/// User registration DTO model.
/// </summary>
public class UserRegistrationDto : UserSignInDto
{
    /// <summary>
    /// Gets or sets user name.
    /// </summary>
    [DefaultValue]
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets role.
    /// </summary>
    [DefaultValue]
    public string Role { get; set; }
}
