using System;

namespace IdentityWebApi.Core.Entities;

/// <summary>
/// Entity base properties.
/// </summary>
public interface IBaseEntity
{
    /// <summary>
    /// Gets or sets entity identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets entity creation date.
    /// </summary>
    public DateTime CreationDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether entity is deleted.
    /// </summary>
    public bool IsDeleted { get; set; }
}
