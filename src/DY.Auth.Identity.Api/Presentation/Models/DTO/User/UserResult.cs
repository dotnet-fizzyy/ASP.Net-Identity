using System;
using System.Collections.Generic;

namespace DY.Auth.Identity.Api.Presentation.Models.DTO.User;

/// <summary>
/// Result "User" model.
/// </summary>
public class UserResult
{
    /// <summary>
    /// Gets or sets user id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets user name.
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
    /// Gets or sets user roles names.
    /// </summary>
    public IEnumerable<string> Roles { get; set; }
}
