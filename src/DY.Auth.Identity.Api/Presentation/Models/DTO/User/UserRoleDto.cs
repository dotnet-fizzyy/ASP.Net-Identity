using System;

namespace DY.Auth.Identity.Api.Presentation.Models.DTO.User;

/// <summary>
/// User role DTO model.
/// </summary>
public class UserRoleDto
{
    /// <summary>
    /// Gets or sets role ID.
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Gets or sets role name.
    /// </summary>
    public string Name { get; set; }
}
