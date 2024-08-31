using System;

namespace DY.Auth.Identity.Api.ApplicationLogic.Services.Role.Queries.GetRoleById;

/// <summary>
/// Get role by ID result model.
/// </summary>
public record GetRoleByIdResult
{
    /// <summary>
    /// Gets role ID.
    /// </summary>
    public Guid RoleId { get; init; }

    /// <summary>
    /// Gets role name.
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    /// Gets concurrency stamp.
    /// </summary>
    public string ConcurrencyStamp { get; init; }

    /// <summary>
    /// Gets creation date.
    /// </summary>
    public DateTime CreationDate { get; init; }
}
