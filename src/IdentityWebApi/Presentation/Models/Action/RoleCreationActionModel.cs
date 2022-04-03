using IdentityWebApi.Presentation.Validation;

namespace IdentityWebApi.Presentation.Models.Action;

public class RoleCreationActionModel
{
    [DefaultValue] 
    public string Name { get; set; }
}