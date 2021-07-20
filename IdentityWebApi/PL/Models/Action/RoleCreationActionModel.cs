using IdentityWebApi.PL.Validation;

namespace IdentityWebApi.PL.Models.Action
{
    public class RoleCreationActionModel
    {
        [DefaultValue]
        public string Name { get; set; }
    }
}