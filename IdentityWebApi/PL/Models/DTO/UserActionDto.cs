using IdentityWebApi.PL.Validation;

namespace IdentityWebApi.PL.Models.DTO
{
    public class UserActionDto : UserDto
    {
        [DefaultValue]
        public string Password { get; set; }
        
        [DefaultValue] 
        public string UserRole { get; set; }
    }
}