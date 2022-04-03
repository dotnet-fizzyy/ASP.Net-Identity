using IdentityWebApi.PL.Validation;

using NetEscapades.Configuration.Validation;

using System.ComponentModel.DataAnnotations;

namespace IdentityWebApi.Startup.Settings;

public class SmtpClientSettings : IValidatable
{
    [Required] 
    public string Host { get; set; }

    [DefaultValue] 
    public int Port { get; set; }

    [Required] 
    public string EmailName { get; set; }

    [Required] 
    public string EmailAddress { get; set; }

    [Required] 
    public string Password { get; set; }

    public bool UseSsl { get; set; }

    public void Validate()
    {
        Validator.ValidateObject(this, new ValidationContext(this), true);
    }
}
