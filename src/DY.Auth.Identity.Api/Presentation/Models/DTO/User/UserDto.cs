using System;
using System.Collections.Generic;

namespace DY.Auth.Identity.Api.Presentation.Models.DTO.User;

/// <summary>
/// User DTO model.
/// </summary>
public class UserDto
{
    /// <summary>
    /// Gets or sets user id.
    /// </summary>
    public Guid Id { get; set; }

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
    public ICollection<UserRoleDto> UserRoles { get; set; }
}
