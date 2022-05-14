using IdentityWebApi.ApplicationLogic.Validation;

using System;

namespace IdentityWebApi.ApplicationLogic.Models.Action;

/// <summary>
/// Base User DTO model.
/// </summary>
public abstract class BaseUserDto
{
    /// <summary>
    /// Gets or sets user id.
    /// </summary>
    [DefaultValue]
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets user name.
    /// </summary>
    [DefaultValue]
    public string UserName { get; set; }

    /// <summary>
    /// Gets or sets concurrency stamp.
    /// </summary>
    [DefaultValue]
    public string ConcurrencyStamp { get; set; }

    /// <summary>
    /// Gets or sets email.
    /// </summary>
    [DefaultValue]
    public string Email { get; set; }

    /// <summary>
    /// Gets or sets phone number.
    /// </summary>
    [DefaultValue]
    public string PhoneNumber { get; set; }
}
