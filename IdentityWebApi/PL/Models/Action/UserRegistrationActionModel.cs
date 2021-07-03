using IdentityWebApi.PL.Validation;

namespace IdentityWebApi.PL.Models.Action
{
    public class UserRegistrationActionModel : UserSignInActionModel
    {
        [DefaultValue]
        public string UserName { get; set; }
        
        [DefaultValue]
        public string Role { get; set; }
    }
}