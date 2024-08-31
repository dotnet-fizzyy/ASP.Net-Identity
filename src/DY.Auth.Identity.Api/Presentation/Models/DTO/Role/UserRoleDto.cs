using System;

namespace DY.Auth.Identity.Api.Presentation.Models.DTO.Role;

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
