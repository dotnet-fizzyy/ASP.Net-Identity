using IdentityWebApi.ApplicationLogic.Validation;

namespace IdentityWebApi.ApplicationLogic.Models.Action;

/// <summary>
/// Role creation DTO.
/// </summary>
public class RoleCreationDto
{
    /// <summary>
    /// Gets or sets role name.
    /// </summary>
    [DefaultValue]
    public string Name { get; set; }
}