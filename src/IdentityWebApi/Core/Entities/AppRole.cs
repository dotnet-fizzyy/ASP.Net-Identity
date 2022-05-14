using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;

namespace IdentityWebApi.Core.Entities;

/// <summary>
/// AppRole entity.
/// </summary>
public class AppRole : IdentityRole<Guid>, IBaseEntity
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
