using IdentityWebApi.ApplicationLogic.Validation;

namespace IdentityWebApi.ApplicationLogic.Models.Action;

public class UserDto : BaseUserDto
{
    [DefaultValue]
    public string Password { get; set; }

    [DefaultValue]
    public string UserRole { get; set; }
}
