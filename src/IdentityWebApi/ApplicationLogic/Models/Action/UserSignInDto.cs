using System.ComponentModel.DataAnnotations;

namespace IdentityWebApi.ApplicationLogic.Models.Action;

public class UserSignInDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}
