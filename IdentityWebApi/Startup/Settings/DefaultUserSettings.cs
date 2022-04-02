using System.ComponentModel.DataAnnotations;

namespace IdentityWebApi.Startup.Settings;

public class DefaultUserSettings
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
