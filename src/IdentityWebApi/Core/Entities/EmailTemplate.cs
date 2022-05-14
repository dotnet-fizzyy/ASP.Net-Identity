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
    /// Gets or sets name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets template layout.
    /// </summary>
    public string Layout { get; set; }

    /// <inheritdoc/>
    public DateTime CreationDate { get; set; }

    /// <inheritdoc/>
    public bool IsDeleted { get; set; }
}
