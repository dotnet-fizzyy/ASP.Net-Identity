using System;

namespace IdentityWebApi.Core.Entities;

/// <summary>
/// EmailTemplate entity.
/// </summary>
public class EmailTemplate : IBaseEntity
{
    /// <inheritdoc/>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets email name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets email subject.
    /// </summary>
    public string Subject { get; set; }

    /// <summary>
    /// Gets or sets email body layout.
    /// </summary>
    public string Layout { get; set; }

    /// <inheritdoc/>
    public DateTime CreationDate { get; set; }

    /// <inheritdoc/>
    public bool IsDeleted { get; set; }
}
