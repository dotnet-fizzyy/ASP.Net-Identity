using System.ComponentModel.DataAnnotations;
using IdentityWebApi.PL.Validation;
using NetEscapades.Configuration.Validation;

namespace IdentityWebApi.Startup.Settings
{
    public class CookiesSettings : IValidatable
    {
        public bool SlidingExpiration { get; set; }
        
        [DefaultValue]
        public int ExpirationMinutes { get; set; }

        public void Validate()
        {
            Validator.ValidateObject(this, new ValidationContext(this), true);
        }
    }
}