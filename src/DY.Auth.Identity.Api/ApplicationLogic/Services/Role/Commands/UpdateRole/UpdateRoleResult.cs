using System;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Commands.UpdateRole;

/// <summary>
/// Update role result model.
/// </summary>
public class UpdateRoleResult
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

    /// <summary>
    /// Gets or sets creation date.
    /// </summary>
    public DateTime CreationDate { get; set; }
}
