using IdentityWebApi.Presentation.Models.DTO;
using IdentityWebApi.Presentation.Validation;

namespace IdentityWebApi.Presentation.Models.Action;

public class UserActionModel : BaseUserDto
{
    [DefaultValue] 
    public string Password { get; set; }

    [DefaultValue] 
    public string UserRole { get; set; }
}
