using System.ComponentModel.DataAnnotations;
using NetEscapades.Configuration.Validation;

namespace IdentityWebApi.Startup.Settings
{
    public class DefaultAdminSettings : IValidatable
    {
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string Role { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), true);
        }
    }
}