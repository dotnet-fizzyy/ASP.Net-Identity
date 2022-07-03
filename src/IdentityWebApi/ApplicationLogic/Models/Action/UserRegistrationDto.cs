namespace IdentityWebApi.ApplicationLogic.Models.Action;

/// <summary>
/// User registration DTO model.
/// </summary>
public class UserRegistrationDto : UserSignInDto
{
    /// <summary>
    /// Gets or sets user name.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets role.
    /// </summary>
    public string Role { get; set; }
}
