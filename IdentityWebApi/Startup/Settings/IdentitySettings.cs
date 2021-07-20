using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NetEscapades.Configuration.Validation;

namespace IdentityWebApi.Startup.Settings
{
    public class IdentitySettings : IValidatable
    {
        [Required]
        public ICollection<string> Roles { get; set; } = new List<string>();
        
        public IdentitySettingsPassword Password { get; set; }

        public ICollection<DefaultUserSettings> DefaultUsers { get; set; } = new List<DefaultUserSettings>();
        
        public EmailSettings Email { get; set; }
        
        public CookiesSettings Cookies { get; set; }
        
        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), true);

            foreach (var user in DefaultUsers)
            {
                user.Validate();
            }
            
            Password.Validate();
            Cookies.Validate();
        }
    }
}