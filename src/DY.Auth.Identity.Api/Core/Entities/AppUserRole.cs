using Microsoft.AspNetCore.Identity;

using System;

namespace DY.Auth.Identity.Api.Core.Entities;

/// <summary>
/// AppUserRole entity.
/// </summary>
public class AppUserRole : IdentityUserRole<Guid>
{
    /// <summary>
    /// Gets or sets <see cref="AppUser"/>.
    /// </summary>
    public AppUser AppUser { get; set; }

    /// <summary>
    /// Gets or sets <see cref="AppRole"/>.
    /// </summary>
    public AppRole Role { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether entity is deleted.
    /// </summary>
    public bool IsDeleted { get; set; }
}
