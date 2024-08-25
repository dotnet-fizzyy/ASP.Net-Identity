namespace DY.Auth.Identity.Api.Presentation.Models.DTO.Role;

/// <summary>
/// Create role DTO.
/// </summary>
public record CreateRoleDto
{
    /// <summary>
    /// Gets or sets role name.
    /// </summary>
    public string Name { get; set; }
}
