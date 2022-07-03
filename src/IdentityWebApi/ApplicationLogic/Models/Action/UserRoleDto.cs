using System;

namespace IdentityWebApi.ApplicationLogic.Models.Action;

/// <summary>
/// UserRole DTO model.
/// </summary>
public class UserRoleDto
{
    /// <summary>
    /// Gets or sets role id.
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Gets or sets user id.
    /// </summary>
    public Guid UserId { get; set; }
}
