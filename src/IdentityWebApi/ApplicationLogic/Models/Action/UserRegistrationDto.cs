using IdentityWebApi.ApplicationLogic.Validation;

namespace IdentityWebApi.ApplicationLogic.Models.Action;

public class UserRegistrationDto : UserSignInDto
{
    [DefaultValue] 
    public string UserName { get; set; }

    [DefaultValue] 
    public string Role { get; set; }
}
