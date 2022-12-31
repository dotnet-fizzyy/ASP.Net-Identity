using System;

namespace IdentityWebApi.ApplicationLogic.Models.Common;

/// <summary>
/// Base "Role" model.
/// </summary>
public abstract class BaseRole
{
    /// <summary>
    /// Gets or sets role id.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets role name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets concurrency stamp.
    /// </summary>
    public string ConcurrencyStamp { get; set; }

    /// <summary>
    /// Gets or sets creation date.
    /// </summary>
    public DateTime CreationDate { get; set; }
}