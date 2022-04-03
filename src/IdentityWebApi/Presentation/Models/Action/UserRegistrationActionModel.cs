using IdentityWebApi.Presentation.Validation;

namespace IdentityWebApi.Presentation.Models.Action;

public class UserRegistrationActionModel : UserSignInActionModel
{
    [DefaultValue] 
    public string UserName { get; set; }

    [DefaultValue] 
    public string Role { get; set; }
}
