using Microsoft.AspNetCore.Identity;

using System;

namespace IdentityWebApi.Core.Entities;

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
}
