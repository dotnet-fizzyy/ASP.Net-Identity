using System.ComponentModel.DataAnnotations;

namespace IdentityWebApi.Presentation.Models.Action;

public class UserSignInActionModel
{
    [Required] 
    [EmailAddress] 
    public string Email { get; set; }

    [Required] 
    public string Password { get; set; }
}
