using System.Collections.Generic;

namespace IdentityWebApi.ApplicationLogic.Models.Action;

/// <summary>
/// User result DTO model.
/// </summary>
public class UserResultDto : BaseUserDto
{
    /// <summary>
    /// Gets or sets user roles names.
    /// </summary>
    public IEnumerable<string> Roles { get; set; } = new List<string>();
}