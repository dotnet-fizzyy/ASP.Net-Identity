using IdentityWebApi.ApplicationLogic.Validation;

namespace IdentityWebApi.ApplicationLogic.Models.Action;

public class RoleCreationDto
{
    [DefaultValue]
    public string Name { get; set; }
}