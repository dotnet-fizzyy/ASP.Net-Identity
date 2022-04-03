using IdentityWebApi.ApplicationLogic.Validation;

using NetEscapades.Configuration.Validation;

using System.ComponentModel.DataAnnotations;

namespace IdentityWebApi.Startup.Settings;

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
