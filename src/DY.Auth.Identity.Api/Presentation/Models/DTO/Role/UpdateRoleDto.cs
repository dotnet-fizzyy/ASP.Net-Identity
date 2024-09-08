using System;

namespace DY.Auth.Identity.Api.Presentation.Models.DTO.Role;

/// <summary>
/// Update role DTO model.
/// </summary>
public class UpdateRoleDto
{
    /// <summary>
    /// Gets or sets role id.
    /// </summary>
    public Guid RoleId { get; set; }

    /// <summary>
    /// Gets or sets role name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets concurrency stamp.
    /// </summary>
    public string ConcurrencyStamp { get; set; }
}
