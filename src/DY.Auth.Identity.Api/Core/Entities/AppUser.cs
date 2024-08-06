using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;

namespace DY.Auth.Identity.Api.Core.Entities;

/// <summary>
/// AppUser entity.
/// </summary>
public class AppUser : IdentityUser<Guid>, IBaseEntity
{
    /// <summary>
    /// Gets or sets user app roles.
    /// </summary>
    public IList<AppUserRole> UserRoles { get; set; }

    /// <inheritdoc/>
    public DateTime CreationDate { get; set; }

    /// <inheritdoc/>
    public bool IsDeleted { get; set; }
}
