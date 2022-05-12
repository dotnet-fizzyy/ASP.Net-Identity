using System.ComponentModel.DataAnnotations;

namespace IdentityWebApi.ApplicationLogic.Models.Action;

/// <summary>
/// User sign in DTO model.
/// </summary>
public class UserSignInDto
{
    /// <summary>
    /// Gets or sets email.
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets password.
    /// </summary>
    [Required]
    public string Password { get; set; }
}
