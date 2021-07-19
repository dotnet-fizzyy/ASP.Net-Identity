using IdentityWebApi.PL.Models.DTO;
using IdentityWebApi.PL.Validation;

namespace IdentityWebApi.PL.Models.Action
{
    public class UserActionModel : BaseUserDto
    {
        [DefaultValue]
        public string Password { get; set; }
        
        [DefaultValue] 
        public string UserRole { get; set; }
    }
}