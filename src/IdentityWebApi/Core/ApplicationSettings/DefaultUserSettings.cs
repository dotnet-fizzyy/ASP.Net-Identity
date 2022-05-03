using System.ComponentModel.DataAnnotations;

namespace IdentityWebApi.Core.ApplicationSettings;

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
