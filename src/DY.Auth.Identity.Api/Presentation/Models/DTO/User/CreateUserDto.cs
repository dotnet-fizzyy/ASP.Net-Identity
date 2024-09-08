using System;

namespace DY.Auth.Identity.Api.Presentation.Models.DTO.User;

/// <summary>
/// Create user DTO model.
/// </summary>
public class CreateUserDto
{
    /// <summary>
    /// Gets or sets user id.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets username.
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets concurrency stamp.
    /// </summary>
    public string ConcurrencyStamp { get; set; }

    /// <summary>
    /// Gets or sets email.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets phone number.
    /// </summary>
    public string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets password.
    /// </summary>
    public string Password { get; set; }

    /// <summary>
    /// Gets or sets user role.
    /// </summary>
    public string UserRole { get; set; }
}
