using System.ComponentModel.DataAnnotations;
using IdentityWebApi.PL.Validation;
using NetEscapades.Configuration.Validation;

namespace IdentityWebApi.Startup.Settings
{
    public class IdentitySettingsPassword : IValidatable
    {
        public bool RequireDigit { get; set; }
        
        public bool RequireLowercase { get; set; }
        
        public bool RequireUppercase { get; set; }
        
        public bool RequireNonAlphanumeric { get; set; }
        
        [DefaultValue]
        public int RequiredLength { get; set; }
        
        [DefaultValue]
        public int RequiredUniqueChars { get; set; }

        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), true);
        }
    }
}