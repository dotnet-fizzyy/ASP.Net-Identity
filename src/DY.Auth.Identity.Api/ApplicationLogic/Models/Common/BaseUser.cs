using System;

namespace DY.Auth.Identity.Api.ApplicationLogic.Models.Common;

/// <summary>
/// Base "User" model.
/// </summary>
public abstract class BaseUser
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
}
